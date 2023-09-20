using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Wallet.DAL.Entities;
using Wallet.Services.Service_Interfaces;
using Wallet.Web.Controllers;
using Wallet.Web;
using Xunit.Sdk;

namespace Wallet.Web.Tests.ControllerTests;

public class AccountManagerTest
{

    private readonly Mock<IAccountManager> _mockAccountManager;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CustomerController _customerController;

    public AccountManagerTest()
    {
        _mockAccountManager = new Mock<IAccountManager>();
        _mockMapper = new Mock<IMapper>();
        _customerController = new CustomerController(_mockAccountManager.Object, _mockMapper.Object);
    }

 
    [Fact]
    public async Task GetAllAsync_ReturnsOkResult_WithListOfCustomer()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new() { Id = 1, FirstName = "John" },
            new() { Id = 2, FirstName = "Ally" }
        };

        var customerViewModels = new List<CustomerViewModel>
        {
            new() { FirstName = "John" },
            new() { FirstName = "Ally" }
        };

        _mockAccountManager.Setup(service => service.GetAllAsync()).ReturnsAsync(customers);
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<CustomerViewModel>>(customers)).Returns(customerViewModels);

        // Act
        var result = await _customerController.GetAllAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCustomerViewModels = Assert.IsAssignableFrom<IEnumerable<CustomerViewModel>>(okResult.Value);
        Assert.Equal(customerViewModels, returnedCustomerViewModels);
    }
    

    [Fact]
    public async Task AddAsync_ValidCustomer_ReturnsOkResult()
    {
        // Arrange
        var customerViewModel = new CustomerViewModel { FirstName = "New Customer" };
        var customer = new Customer { FirstName = "New Customer" };

        _mockMapper.Setup(mapper => mapper.Map<Customer>(customerViewModel)).Returns(customer);

        // Act
        var result = await _customerController.AddAsync(customerViewModel);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
    }


    [Fact]
    public async Task AddAsync_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        _customerController.ModelState.AddModelError("Name", "Name is required.");
        var invalidCustomerViewModel = new CustomerViewModel();

        // Act
        var result = await _customerController.AddAsync(invalidCustomerViewModel);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
    }
    [Fact]
    public async Task GetByAccountNumber_Returns_CorrectCustomer()
    {
        //arrange
        var customer = new List<Customer>
        {
            new()
            {
                FirstName = "John", LastName = "Doe", Balance = 10.0m,
                AccountNum = "1234567890", Email = "john@up.com"
            },
            new()
            {
                FirstName = "Ally", LastName = "Doe", Balance = 20.0m,
                AccountNum = "2150005647", Email = "ally@up.com"
            }
        };
        //act
        _mockAccountManager.Setup(x => x.GetByAccountNumber("1234567890"))
            .ReturnsAsync(customer.Find(c => c.AccountNum == "1234567890"));
        var result = await _customerController.GetByAccountNumber("1234567890");
        
        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actual = Assert.IsType<Customer>(okResult.Value);
        //Assert.Equal(customer, actual);
        Assert.Equal("John", actual.FirstName);

    }
}

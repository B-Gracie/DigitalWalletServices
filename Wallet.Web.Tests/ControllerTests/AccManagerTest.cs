using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Wallet.DAL.Entities;
using Wallet.Services.Service_Interfaces;
using Wallet.Web.Controllers;

namespace Wallet.Web.Tests.ControllerTests;

public class AccountManagerTest
{
    private readonly Mock<IAccountManager> _servicesMock;

    private readonly IMapper _mapMock;

    private readonly CustomerController _controller;

    //= new();

    public AccountManagerTest()
    {
        
         _servicesMock = new Mock<IAccountManager>();
        //var mockMapper = new Mock<IMapper>();
        //_controller = new CustomerController(_servicesMock.Object, mockMapper.Object);
        //
        //_servicesMock = new Mock<IAccountManager>();

        _mapMock = new MapperConfiguration (cfg => { cfg.AddProfile(new CustomerProfile()); }).CreateMapper();

        //_controller = new CustomerController(_servicesMock.Object, _mapMock);
    }
    
    [Fact]
    public async Task AddAsync_ReturnsOkResult()
    {
        // Arrange
        var mockService = new Mock<IAccountManager>();
        var mockMapper = new Mock<IMapper>();
        var controller = new CustomerController(mockService.Object, mockMapper.Object);

        var customerViewModel = new CustomerViewModel
        {
            FirstName = "James",
            LastName = "Test",
            Email = "james@test.com",
            Username = "JaT",
            Password = "test",
            AccountNum = "1234567890",
            Balance = 50
        };
        var customer = new Customer
        {
            FirstName = "James",
            LastName = "Test",
            Email = "james@test.com",
            Username = "JaT",
            Password = "test",
            AccountNum = "1234567890",
            Balance = 50
        };
        mockMapper.Setup(m => m.Map<Customer>(customerViewModel)).Returns(customer);
        mockService.Setup(s => s.AddAsync(It.IsAny<Customer>())).ReturnsAsync(customer);

        // Act
        var result = await controller.AddAsync(customerViewModel);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        mockMapper.Verify(m => m.Map<Customer>(customerViewModel), Times.Once);
        mockService.Verify(s => s.AddAsync(It.IsAny<Customer>()), Times.Once);
    }



    [Fact]
    public async Task GetAllAsync_ReturnsOkResult_WithExpectedCustomers()
{
    // Arrange
    var expectedCustomers = new List<Customer>
    {
        new()
        {
            FirstName = "James",
            LastName = "Test",
            Email = "james@test.com",
            Username = "JaT",
            Password = "test",
            AccountNum = "1234567890",
            Balance = 50

        },
        
        new()
        {
            FirstName = "Team",
            LastName = "Bukola",
            Email = "team@gmail.com",
            Password = "0909457637",
            Username = "bukolag",
            AccountNum = "122",
            Balance = 100
        },
    };
    var mockService = new Mock<IAccountManager>();
    mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(expectedCustomers);

    var mockMapper = new Mock<IMapper>();
    var expectedViewModels = expectedCustomers.Select(c => new CustomerViewModel
    {
        FirstName = c.FirstName,
        LastName = c.LastName,
        Email = c.Email
    }).ToList();
    mockMapper.Setup(m => m.Map<IEnumerable<CustomerViewModel>>(expectedCustomers))
        .Returns(expectedViewModels);

    var controller = new CustomerController(mockService.Object, mockMapper.Object);

    // Act
    var result = await controller.GetAllAsync();

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var actualViewModels = Assert.IsAssignableFrom<IEnumerable<CustomerViewModel>>(okResult.Value);
    Assert.Equal(expectedViewModels.Count, actualViewModels.Count());
    for (int i = 0; i < expectedViewModels.Count; i++)
    {
        Assert.Equal(expectedViewModels[i].FirstName, actualViewModels.ElementAt(i).FirstName);
        Assert.Equal(expectedViewModels[i].LastName, actualViewModels.ElementAt(i).LastName);
        //Assert.Equal(expectedViewModels[i].Email, actualViewModels.ElementAt(i).Email);
    }

    mockService.Verify(s => s.GetAllAsync(), Times.Once);
    mockMapper.Verify(m => m.Map<IEnumerable<CustomerViewModel>>(expectedCustomers), Times.Once);
}

    
}



/*[Fact]
public async Task AddAsync_ReturnsOkResult_WhenAddSucceeds()
{
    var customerinfo = new CustomerViewModel()
    {
        FirstName = "Team",
        LastName = "Bukola",
        Email = "team@gmail.com",
        Password = "0909457637",
        Username = "bukolag",
        AccountNum = "122",
        Balance = 100
    };

    Customer user = _mapMock.Map<Customer>(customerinfo);
    
    _servicesMock.Setup(m => m.AddAsync(user)).ReturnsAsync(user);
    var result =  _controller.AddAsync(customerinfo);
    
    //Assert
    Assert.NotNull(result);
}*/
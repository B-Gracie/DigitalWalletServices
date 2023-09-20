using Microsoft.AspNetCore.Mvc;
using Moq;
using Wallet.TransactionsAPI.TransactionInterface;
using Wallet.Web.Controllers;

namespace Wallet.Web.Tests.ControllerTests;

public class TransactionControllerTests
{
    private readonly TransactionController _controller;
    private readonly Mock<ITransactionService> _transactionServiceMock;

    public TransactionControllerTests()
    {
        _transactionServiceMock = new Mock<ITransactionService>();
        _controller = new TransactionController(_transactionServiceMock.Object);
    }

    [Fact]
    public async Task GetAccountBalance_Should_Return_OkResult_With_Balance()
    {
        // Arrange
        string accountNumber = "1234567890";
        decimal balance = 100.50m;

        _transactionServiceMock.Setup(x => x.GetAccountBalanceAsync(accountNumber)).ReturnsAsync(balance);

        // Act
        var result = await _controller.GetAccountBalance(accountNumber);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(balance, okResult.Value);
    }

    [Fact]
    public async Task GetAccountBalance_Should_Return_NotFoundResult_When_AccountNotFound()
    {
        // Arrange
        string accountNumber = "invalid_account_number";
        string errorMessage = $"Account with account number {accountNumber} not found.";
        _transactionServiceMock.Setup(x => x.GetAccountBalanceAsync(accountNumber))
            .ThrowsAsync(new ArgumentException(errorMessage));

        // Act
        var result = await _controller.GetAccountBalance(accountNumber);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(errorMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task Deposit_Should_Return_OkResult_With_DepositResponseModel()
    {
        // Arrange
        var requestModel = new DepositRequestModel
        {
            AccountNum = "123456",
            Amount = 50.0m
        };

        var depositAmount = requestModel.Amount;
        var txnTime = DateTime.UtcNow;

        _transactionServiceMock.Setup(x => x.DepositAsync(requestModel.AccountNum, requestModel.Amount))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Deposit(requestModel);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseModel = Assert.IsType<DepositResponseModel>(okResult.Value);
        Assert.Equal(depositAmount, responseModel.DepositAmount);
        Assert.Equal(txnTime, responseModel.TxnTime,
            TimeSpan.FromSeconds(1)); 
    }
}
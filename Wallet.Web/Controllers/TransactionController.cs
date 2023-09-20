using Microsoft.AspNetCore.Mvc;
using Wallet.TransactionsAPI.TransactionInterface;


namespace Wallet.Web.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _service;

    public TransactionController(ITransactionService service)
    {
        _service = service;
    }


    [HttpGet("accountBalance/{accountNumber}")]
    public async Task<IActionResult> GetAccountBalance(string accountNumber)
    {
        try
        {
            var balance = await _service.GetAccountBalanceAsync(accountNumber);
            return Ok(balance);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] DepositRequestModel requestModel)
    {
        try
        {
            await _service.DepositAsync(requestModel.AccountNum, requestModel.Amount);

            var responseModel = new DepositResponseModel
            {
                DepositAmount = requestModel.Amount,
                TxnTime = DateTime.UtcNow 
            };

            return Ok(responseModel);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            // Handle other exceptions if needed.
            return StatusCode(500, "An error occurred during the deposit process.");
        }
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] WithdrawalRequestModel requestModel)
    {
        try
        {
            var withdrawalResponse = await _service.WithdrawAsync(requestModel.AccountNum, requestModel.Amount);

            return Ok(withdrawalResponse);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            // Handle other exceptions if needed.
            return StatusCode(500, "An error occurred during the withdrawal process.");
        }

    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllTransactions()
    {
        try
        {
            var transactions = await _service.GetAllTransactionsAsync();
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            // Handle exceptions if needed.
            return StatusCode(500, "An error occurred while fetching the transactions.");
        }
    }
}
    
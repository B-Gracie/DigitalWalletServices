using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Wallet.TransactionsAPI.TransactionInterface;
using Npgsql;


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
        

        [HttpGet("{accountNumber}/balance")]
        public async Task<ActionResult<decimal>> GetAccountBalanceAsync(string accountNumber)
        {
            try
            {
                decimal balance = await _service.GetAccountBalanceAsync(accountNumber);

                return Ok(balance);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        
        /*[HttpGet("{accountNumber}/balance")]
        public async Task<ActionResult<decimal>> GetAccountBalanceAsync(string accountNumber)
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }*/

        [HttpPost("{accountNumber}/deposit")]
        public async Task<ActionResult> DepositAsync(string accountNumber, [FromBody] decimal amount)
        {
            try
            {
                await _service.DepositAsync(accountNumber, amount);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("{accountNumber}/withdraw")]
        public async Task<ActionResult> WithdrawAsync(string accountNumber, [FromBody] decimal amount)
        {
            try
            {
                await _service.WithdrawAsync(accountNumber, amount);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
    
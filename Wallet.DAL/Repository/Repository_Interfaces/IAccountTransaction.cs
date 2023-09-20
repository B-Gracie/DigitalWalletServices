using System.Transactions;
using Wallet.DAL.Entities;

namespace Wallet.DAL.Repository;

public interface IAccountTransaction
{
     Task<decimal> GetAccountBalanceAsync(string accountNumber);
     Task DepositAsync(string accountNumber, decimal amount);
     Task<WithdrawalResponseModel> WithdrawAsync(string accountNumber, decimal amount);
     Task<List<AccountTransaction>> GetAllTransactionsAsync();

}
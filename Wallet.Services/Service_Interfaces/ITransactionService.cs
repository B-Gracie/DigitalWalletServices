using Wallet.DAL.Entities;

namespace Wallet.TransactionsAPI.TransactionInterface;

public interface ITransactionService
{
  
        Task<decimal> GetAccountBalanceAsync(string accountNumber);
        Task DepositAsync(string accountNumber, decimal amount);
        Task WithdrawAsync(string accountNumber, decimal amount);
        public Task<AccountTransaction> AddAsync(AccountTransaction txnInfo);

        
   }

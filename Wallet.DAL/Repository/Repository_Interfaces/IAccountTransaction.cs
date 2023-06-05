using System.Transactions;
using Wallet.DAL.Entities;

namespace Wallet.DAL.Repository;

public interface IAccountTransaction
{
     Task<decimal> GetAccountBalanceAsync(string accountNumber);
     Task<AccountTransaction> GetAccountByAccountNumberAsync(string accountNumber);
     Task DepositAsync(AccountTransaction account, decimal amount);
     Task WithdrawAsync(AccountTransaction account, decimal amount);

     public Task AddAsync(AccountTransaction txnInfo);
     public Task SaveChangesAsync();
     void UpdateAsync(AccountTransaction txnInfo);


     //void Update(AccountTransaction accTxn);


}
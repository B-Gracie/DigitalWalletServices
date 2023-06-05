using Wallet.DAL.Entities;
using Wallet.DAL.Repository;
using Wallet.Services.Service_Interfaces;
using Wallet.TransactionsAPI.TransactionInterface;

namespace Wallet.Services.Wallet_Services;

public class Transaction
{
    public class TransactionService : ITransactionService
    {
        private readonly IAccountTransaction _repository;

        public TransactionService(IAccountTransaction repository)
        {
            _repository = repository;
        }

        public async Task<decimal> GetAccountBalanceAsync(string accountNumber)
        {
            
                var balance = await _repository.GetAccountBalanceAsync(accountNumber);

                if (balance == null)
                {
                    throw new ArgumentException($"Account with account number {accountNumber} not found.");
                }

                return balance;
            }

        
        public async Task DepositAsync(string accountNumber, decimal amount)
        {
            var account = await _repository.GetAccountByAccountNumberAsync(accountNumber);

            await _repository.DepositAsync(account, amount);
            
            if (account == null)
            {
                throw new ArgumentException($"Account with number {accountNumber} not found.");
            }
            account.Balance += amount;
            _repository.UpdateAsync(account);
        }

        public async Task WithdrawAsync(string accountNumber, decimal amount)
        {
            var account = await _repository.GetAccountByAccountNumberAsync(accountNumber);

            await _repository.WithdrawAsync(account, amount);
            if (account == null)
            {
                throw new ArgumentException($"Account with number {accountNumber} not found.");
            }
            if (account.Balance < amount)
            {
                throw new InvalidOperationException("Insufficient funds.");
            }
            account.Balance -= amount;
            _repository.UpdateAsync(account);
        }

        public async Task<AccountTransaction> AddAsync(AccountTransaction txnInfo)
        {
            await _repository.AddAsync(txnInfo);
            await _repository.SaveChangesAsync();
            return (txnInfo);
        }
    }
}
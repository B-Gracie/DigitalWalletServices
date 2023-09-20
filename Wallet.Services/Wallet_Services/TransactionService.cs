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
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be greater than zero.");
            }

            await _repository.DepositAsync(accountNumber, amount);
            
        }
        public async Task<WithdrawalResponseModel> WithdrawAsync(string accountNumber, decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be greater than zero.");
            }

            var withdrawalResponse = await _repository.WithdrawAsync(accountNumber, amount);

            return withdrawalResponse;
        }
        
        public async Task<List<AccountTransaction>> GetAllTransactionsAsync()
        {
            return await _repository.GetAllTransactionsAsync();
        }
    }
}


using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Wallet.DAL.Entities;

namespace Wallet.DAL.Repository;

public class AccountTransactionsRepository
{
    public class AccountTransactionRepository : IAccountTransaction
    {
        private readonly WalletContext _walletContext;


        public AccountTransactionRepository(WalletContext walletContext)
        {
            _walletContext = walletContext;

        }
 
        public async Task<decimal> GetAccountBalanceAsync(string accountNumber)
        {
            var customer = await _walletContext.Customers
                    .FirstOrDefaultAsync(c => c.AccountNum == accountNumber);

                // Return the account balance or 0 if no customer is found
                return customer?.Balance ?? 0;
            }

        public async Task DepositAsync(string accountNumber, decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be greater than zero.");
            }

            var existingAccount = await _walletContext.Customers
                .FirstOrDefaultAsync(a => a.AccountNum == accountNumber);

            if (existingAccount == null)
            {
                throw new ArgumentException($"Account with account number {accountNumber} not found.");
            }

            existingAccount.Balance += amount;

            await _walletContext.SaveChangesAsync();

            var transaction = new AccountTransaction
            {
                CustomerId = existingAccount.Id, 
                AccountNum = accountNumber,
                DepositAmount = amount,
                TxnTime = DateTime.UtcNow 
            };

            _walletContext.AccountTransactions.Add(transaction);
            await _walletContext.SaveChangesAsync();
            
        }
        public async Task<WithdrawalResponseModel> WithdrawAsync(string accountNumber, decimal amount)
        {
        

            var existingAccount = await _walletContext.Customers
                .FirstOrDefaultAsync(a => a.AccountNum == accountNumber);

            if (existingAccount == null)
            {
                throw new ArgumentException($"Account with account number {accountNumber} not found.");
            }

         
            if (existingAccount.Balance < amount)
            {
                throw new ArgumentException($"Insufficient balance in account with account number {accountNumber}.");
            }

            existingAccount.Balance -= amount;

            var transaction = new AccountTransaction
            {
                CustomerId = existingAccount.Id,
                AccountNum = accountNumber,
                WithdrawalAmount = amount,
                TxnTime = DateTime.UtcNow 

            };
            _walletContext.AccountTransactions.Add(transaction);
            _walletContext.SaveChanges();

            // Return a response object containing the updated balance
            //return new WithdrawalResponseModel { UpdatedBalance = existingAccount.Balance };
            return new WithdrawalResponseModel
            {
                WithdrawalAmount = amount,
                TxnTime = transaction.TxnTime,
                UpdatedBalance = existingAccount.Balance
            };
        }
            // var withdrawalResponse = new WithdrawalResponseModel
            // {
            //     WithdrawalAmount = amount,
            //     TxnTime = DateTime.UtcNow,
            //     UpdatedBalance = existingAccount.Balance
            // };

            // await _walletContext.SaveChangesAsync();
            //
            // return withdrawalResponse;

            public async Task<List<AccountTransaction>> GetAllTransactionsAsync()
        {
            return await _walletContext.AccountTransactions.ToListAsync();
        }   
    }
    }







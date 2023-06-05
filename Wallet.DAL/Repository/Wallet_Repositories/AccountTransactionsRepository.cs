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
            await using var connection = new NpgsqlConnection("Host=127.0.0.1; Port=5432; Database=postgres; Username=user; Password=admin");
            await connection.OpenAsync();

            var command =
                new NpgsqlCommand("SELECT Balance FROM Users.Transactions  WHERE AccountNum = @accountNumber",
                    connection);
            command.Parameters.AddWithValue("accountNumber", accountNumber);

            var balance = await command.ExecuteScalarAsync();

            if (balance == null || balance == DBNull.Value)
            {
                throw new ArgumentException($"Account with account number {accountNumber} not found.");
            }

            return Convert.ToDecimal(balance);
        }


        public async Task<AccountTransaction> GetAccountByAccountNumberAsync(string accountNumber)
        {
            var account = await _walletContext.AccountTransactions.SingleOrDefaultAsync
                (a => a.AccountNum == accountNumber);

            if (account == null)
            {
                throw new ArgumentException($"Account with account number {accountNumber} not found.");
            }

            return account;
        }

        public async Task DepositAsync(AccountTransaction account, decimal amount)
        {
            account.Balance += amount;
            await _walletContext.SaveChangesAsync();
        }

        public async Task WithdrawAsync(AccountTransaction account, decimal amount)
        {
            if (account.Balance < amount)
            {
                throw new InvalidOperationException($"Insufficient funds. Account balance is {account.Balance:C}, but withdrawal amount is {amount:C}.");
            }

            account.Balance -= amount;
            await _walletContext.SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _walletContext.SaveChangesAsync();
        }

        public void UpdateAsync(AccountTransaction txnInfo)
        {
             _walletContext.AccountTransactions.Update(txnInfo);
        }

        public async Task AddAsync(AccountTransaction txnInfo)
        {
        
            await _walletContext.AccountTransactions.AddAsync(txnInfo);
        
        }
    }
    }







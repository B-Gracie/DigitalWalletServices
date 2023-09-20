using FluentAssertions;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Wallet.DAL.Entities;
using Wallet.DAL.Repository;
using Wallet.Migrations.Migrations;
using PostgreSqlContainer = Testcontainers.PostgreSql.PostgreSqlContainer;

namespace Wallet.DAL.Test.RepositoryTest;

    public sealed class AccountTransactionRepositoryTests : IAsyncLifetime
    {
        private WalletContext _dbContext;
        private IAccountTransaction _accountTransactionRepository;


        private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();

        public async Task InitializeAsync()
        {
            //return _postgreSqlContainer.StartAsync();
            await _postgreSqlContainer.StartAsync();

            // Create the database context and apply migrations
            _dbContext = CreateDbContext();

            // Create the account transaction repository with the database context
            _accountTransactionRepository = new AccountTransactionsRepository.AccountTransactionRepository(_dbContext);
        }


   

        private WalletContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<WalletContext>()
                .UseNpgsql(_postgreSqlContainer.GetConnectionString())
                .Options;

            var dbContext = new WalletContext(options);
            
            var serviceProvider = CreateServices();
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }

            return dbContext;
        }

        
        private IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(_postgreSqlContainer.GetConnectionString())
                    .ScanIn(typeof(CustomersTable).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        [Fact]
        public async Task GetAccountBalanceAsync_ShouldReturnAccountBalance()
        {
            // Arrange
            var accountNumber = "1234567890";
            var firstName = "Ally";
            var lastName = "Doe";
            var userName = "AllyDoe";
            var Id = 1;
            var passWord = "Ally";
            var email = "buki@up.com";
            var balance = 60.0m;
            var customer = new Customer
            {
                AccountNum = accountNumber, Balance = balance,
                FirstName = firstName, Email = email,
                Password = passWord, LastName = lastName, Username = userName, Id = Id
            };

            using (var dbContext = CreateDbContext())
            {
                dbContext.Customers.Add(customer);
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = CreateDbContext())
            {
                var accountTransactionRepository =
                    new AccountTransactionsRepository.AccountTransactionRepository(dbContext);

                // Act
                var result = await accountTransactionRepository.GetAccountBalanceAsync(accountNumber);

                // Assert
                Assert.Equal(balance, result);
            }
        }


        [Fact]
        public async Task DepositAsync_ShouldIncreaseBalanceAndAddTransaction()
        {
            // Arrange
            var accountNumber = "1234567890";
            var amountToDeposit = 500.0m;
            var lastName = "Doe";
            var userName = "AllyDoe";
            var id = 1;
            var passWord = "Ally";
            var email = "buki@up.com";
            //var balance = 60.0m; 
            var firstName = "Ally";

            var customer = new Customer
            {
                AccountNum = accountNumber, FirstName = firstName,
                LastName = lastName, Username = userName, Id = id, Password = passWord, Email = email,
                Balance = 1000.0m
            };
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();

            // Create the account transaction repository with the database context
            _accountTransactionRepository = new AccountTransactionsRepository.AccountTransactionRepository(_dbContext);

            // Act
            await _accountTransactionRepository.DepositAsync(accountNumber, amountToDeposit);

            // Assert
            var existingAccount = await _dbContext.Customers.FirstOrDefaultAsync
                (a => a.AccountNum == accountNumber);
            Assert.NotNull(existingAccount);
            Assert.Equal(1500.0m, existingAccount.Balance);

            var transactions = await _dbContext.AccountTransactions.ToListAsync();
            Assert.Single(transactions); // Assuming only one transaction was added
            Assert.Equal(accountNumber, transactions[0].AccountNum);
            Assert.Equal(amountToDeposit, transactions[0].DepositAmount);
        }

        [Fact]
        public async Task WithdrawAsync_ShouldDecreaseBalanceAndAddTransaction()
        {
            // Arrange
            var accountNumber = "1234567890";
            var amountToDeposit = 500.0m;
            var lastName = "Doe";
            var userName = "AllyDoe";
            var id = 1;
            var passWord = "Ally";
            var email = "buki@up.com";
            //var balance = 60.0m; 
            var firstName = "Ally";
            var amountToWithdraw = 200.0m; // Replace with the withdrawal amount
            var customer = new Customer
            {
                AccountNum = accountNumber, Balance = 1000.0m, Email = email,
                LastName = lastName, Username = userName, Id = id, Password = passWord,
                FirstName = firstName
            };
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();

            // Act
            var accountTransactionRepository =
                new AccountTransactionsRepository.AccountTransactionRepository(_dbContext);
            var withdrawalResponse = await accountTransactionRepository.WithdrawAsync(accountNumber, amountToWithdraw);

            // Assert
            Assert.Equal(800.0m, withdrawalResponse.UpdatedBalance); // Replace with the expected new balance

            var transactions = await _dbContext.AccountTransactions.ToListAsync();
            Assert.Single(transactions); 
            Assert.Equal(accountNumber, transactions[0].AccountNum);
            Assert.Equal(amountToWithdraw, transactions[0].WithdrawalAmount);
        }

        [Fact]
        public async Task GetAllTransactionsAsync_ShouldReturnAllTransactions()
        {
            // Arrange

            var customer1 = new Customer { AccountNum = "1234567890", Balance = 1000.0m, 
                Email = "buki@up.com", LastName = "Doe", Username = "buki", Id = 1,  Password = "buki",
                FirstName = "Buki"};
            var customer2 = new Customer { Id = 2, AccountNum = "1234567890", 
                Balance = 1000.0m, Email = "buki@up.com",
                LastName = "Doe", Username = "Ally",  Password = "Ally",
                FirstName = "Ally"};
            
            _dbContext.Customers.AddRange(customer1, customer2);
            await _dbContext.SaveChangesAsync();
            var testData = new List<AccountTransaction>
            {
                new() { CustomerId = 1,  AccountNum = "1234567890", TxnTime = DateTime.UtcNow, WithdrawalAmount = 100 },
                new() { CustomerId = 2,  AccountNum = "2150005647", TxnTime = DateTime.UtcNow, WithdrawalAmount = 200 },
            };

            _dbContext.AccountTransactions.AddRange(testData);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _accountTransactionRepository.GetAllTransactionsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(testData.Count);

            var firstTransaction = result.First();
            firstTransaction.CustomerId.Should().Be(1);
            firstTransaction.AccountNum.Should().Be("1234567890");
            firstTransaction.WithdrawalAmount.Should().Be(100);
        }

        public async Task DisposeAsync()
        {
            await _postgreSqlContainer.DisposeAsync();
        }
    }






    /*private WalletContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<WalletContext>()
            .UseNpgsql(_postgreSqlContainer.GetConnectionString())
            .Options;

        var dbContext = new WalletContext(options);

        // Ensure that the database is created and apply migrations
        dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();

        return dbContext;
    }*/

        // private WalletContext CreateDbContext()
        // {
        //     var options = new DbContextOptionsBuilder<WalletContext>()
        //         .UseNpgsql(_postgreSqlContainer.GetConnectionString())
        //         .Options;
        //
        //     return new WalletContext(options);
        // }
    

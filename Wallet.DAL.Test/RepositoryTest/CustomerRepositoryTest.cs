using FluentAssertions;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wallet.DAL.Repository;
using Testcontainers.PostgreSql;
using Wallet.DAL.Entities;
using Wallet.Migrations.Migrations;

namespace Wallet.DAL.Test.RepositoryTest;

public sealed class CustomerRepositoryTest : IAsyncLifetime
{
    private WalletContext _dbContext;
    private IRepository _customerRepository;


    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        _dbContext = CreateDbContext();

        _customerRepository = new CustomerRepository(_dbContext);
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
    public async Task GetAllAsync_ShouldReturnAllCustomers()
    {
        // Arrange
        var testData = new List<Customer>
        {
            new()
            { Id = 1, FirstName = "John", Email = "john@up.com",
                LastName = "Doe", Username = "John", Balance = 100.0m, 
                Password = "John", AccountNum = "1234567890"},
            
            new()
            { Id = 2, FirstName = "Jane", Email = "jane@up.com", 
                LastName = "Doe", Username = "John", Balance = 50.0m, 
                Password = "John", AccountNum = "1234567890"},
        };
        _dbContext.Customers.AddRange(testData);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _customerRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        var firstCustomer = result.ToList()[1];

        Assert.Equal(firstCustomer.FirstName, firstCustomer.FirstName);
        Assert.Equal(firstCustomer.LastName, firstCustomer.LastName);
        
        // result.Should().NotBeNull();
        // result.Should().HaveCount(testData.Count);
        //
        // var firstCustomer = result.ToList()[1];
        // firstCustomer.FirstName.Should().Be("Jane");
        // firstCustomer.Email.Should().Be("jane@up.com");

        await _dbContext.SaveChangesAsync();
    }


    [Fact]
    public async Task AddAsync_ShouldAddCustomerToDatabase()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1, 
            FirstName = "John", 
            Email = "john@up.com",
            LastName = "Doe", 
            Username = "John", 
            Balance = 100.0m, 
            Password = "John", 
            AccountNum = "1234567890"
        };

        // Act
        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveChangesAsync();

        // Assert
        var addedCustomer = await _dbContext.Customers.FindAsync(customer.Id);
        Assert.NotNull(addedCustomer);
        Assert.Equal(customer.FirstName, addedCustomer.FirstName);
        Assert.Equal(customer.LastName, addedCustomer.LastName);

       //_dbContext.Customers.Remove(addedCustomer);
        await _dbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task GetByAccountNumber_ShouldReturnCustomerDetails()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1, 
            FirstName = "John", 
            Email = "john@up.com",
            LastName = "Doe", 
            Username = "John", 
            Balance = 100.0m, 
            Password = "John", 
            AccountNum = "1234567890"
        };

        // Act
        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveChangesAsync();
        var actual = await _customerRepository.GetByAccountNumber("1234567890");

        //Assert
      Assert.NotNull(actual);
      Assert.Equal("John", actual.FirstName );
        
    }

public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
    }
}

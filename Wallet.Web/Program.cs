using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wallet.DAL;
using Wallet.DAL.Entities;
using Wallet.DAL.Repository;
using Wallet.Services.Service_Interfaces;
using Wallet.Services.Wallet_Services;
using Wallet.TransactionsAPI.TransactionInterface;
using Wallet.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = new ConfigurationBuilder()
    .AddJsonFile("app" + "settings.json")
    .Build();


builder.Services.AddDbContext<WalletContext>(options =>
{
    options.UseNpgsql(config.GetConnectionString("Users"));
});
builder.Services.AddAutoMapper(typeof(CustomerProfile).Assembly);
// In your AutoMapper configuration:
MapperConfiguration mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Customer, CustomerViewModel>();
});
var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


builder.Services.AddScoped<IAccountManager, AccountManagerService.CustomerManagerService>();

builder.Services.AddScoped<IRepository, CustomerRepository>();

builder.Services.AddScoped< ITransactionService, Transaction.TransactionService >();
builder.Services.AddScoped<IAccountTransaction, AccountTransactionsRepository.AccountTransactionRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
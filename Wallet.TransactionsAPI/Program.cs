using Microsoft.EntityFrameworkCore;
using Wallet.DAL;
using Wallet.DAL.Repository;
using Wallet.Services.Wallet_Services;
using Wallet.TransactionsAPI.TransactionInterface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add services to the container.
var config = new ConfigurationBuilder()
    .AddJsonFile("app" + "settings.json")
    .Build();

builder.Services.AddDbContext<WalletContext>(options =>
{
    options.UseNpgsql(config.GetConnectionString("Users"));
});
builder.Services.AddScoped<ITransactionService, Transaction.TransactionService>();

builder.Services.AddScoped<IAccountTransaction, AccountTransactionsRepository.AccountTransactionRepository>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
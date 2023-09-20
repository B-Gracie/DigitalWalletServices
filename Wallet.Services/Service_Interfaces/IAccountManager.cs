using Wallet.DAL.Entities;

namespace Wallet.Services.Service_Interfaces;

public interface IAccountManager
{
    
    public Task<IEnumerable<Customer>> GetAllAsync();
    //public Task<Customer> GetByIdAsync(int Userid);
    public Task<Customer> AddAsync(Customer customerinfo);
    public Task<Customer?> GetByAccountNumber(string accountNum);
}
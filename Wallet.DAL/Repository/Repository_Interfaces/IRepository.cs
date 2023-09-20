using Wallet.DAL.Entities;

namespace Wallet.DAL.Repository;

public interface IRepository
{
     public Task SaveChangesAsync();
    public Task<IEnumerable<Customer>> GetAllAsync();
    public Task<Customer?> GetByAccountNumber(string accountNum);
    public Task AddAsync(Customer customerinfo);
}


// public Task SaveChangesAsync();
// public Task<IEnumerable<Customer>> GetAllCustomersAsync();
// Task<IEnumerable<Account>> GetAllAccountsAsync();
// public Task<Transaction> GetByTxnIdAsync(Guid txnid);
// public Task AddCustomerAsync(Customer customerInfo);

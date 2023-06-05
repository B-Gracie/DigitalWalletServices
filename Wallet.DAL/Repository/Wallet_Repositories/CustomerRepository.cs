using Microsoft.EntityFrameworkCore;
using Wallet.DAL.Entities;

namespace Wallet.DAL.Repository;

public class CustomerRepository : IRepository
{
    private readonly WalletContext _context;

    public CustomerRepository(WalletContext context)
    {
        _context = context;
    }
        
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
        
    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer> GetByIdAsync(int userid)
    {
        return await _context.Set<Customer>().FindAsync(userid);
    }

    public async Task AddAsync(Customer customerinfo)
    {
        
        await _context.Customers.AddAsync(customerinfo);
        
    }
}
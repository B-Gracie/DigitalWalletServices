using Microsoft.EntityFrameworkCore;
using Wallet.DAL.Entities;

namespace Wallet.DAL;

public class WalletContext : DbContext
    {
        public WalletContext(DbContextOptions<WalletContext> options)
                : base(options)
            {
            }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<AccountTransaction> AccountTransactions { get; set; }
    }

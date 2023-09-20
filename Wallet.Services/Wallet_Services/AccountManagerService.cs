using Wallet.DAL.Entities;
using Wallet.DAL.Repository;
using Wallet.Services.Service_Interfaces;

namespace Wallet.Services.Wallet_Services;

public class AccountManagerService
{
    public class CustomerManagerService : IAccountManager
    {
        private readonly IRepository _customerRepo;

        public CustomerManagerService(IRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }


        public Task<IEnumerable<Customer>> GetAllAsync()
        {
            return _customerRepo.GetAllAsync();
        }
        

        public Task<Customer> GetByAccountNumber (string accountNum)
        {
            return _customerRepo.GetByAccountNumber(accountNum);
        }


        public async Task <Customer> AddAsync(Customer customer)
        {
           await _customerRepo.AddAsync(customer);
            await _customerRepo.SaveChangesAsync();
            return (customer);
        }
    }
}
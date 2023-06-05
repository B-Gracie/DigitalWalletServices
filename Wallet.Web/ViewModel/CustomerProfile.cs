using AutoMapper;
using Wallet.DAL.Entities;

namespace Wallet.Web;

public class CustomerProfile : Profile
{
    public CustomerProfile()
        {
          // CreateMap<CustomerViewModel, Customer >();
           CreateMap<Customer, CustomerViewModel>();

        }
}

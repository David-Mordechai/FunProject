using FunProject.Application.CustomersModule.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunProject.Application.CustomersModule.Services.Interfaces
{
    public interface ICustomersService
    {
        Task<IList<CustomerDto>> GetAllCustomers();
        Task<CustomerDto> GetCustomer(int? id);
        Task<CustomerDto> CreateCustomer(CustomerDto customer);
        Task DeleteCustomer(int? id);
    }
}
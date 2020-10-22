using FunProject.Domain.Entities;
using System.Threading.Tasks;

namespace FunProject.Application.Data.Customers.Command
{
    public interface ICreateCustomerCommand
    {
        Task<Customer> Create(Customer customer);
    }
}

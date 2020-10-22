using FunProject.Domain.Entities;
using System.Threading.Tasks;

namespace FunProject.Application.Data.Customers.Command
{
    public interface IDeleteCustomerCommand
    {
        Task Delete(Customer customer);
    }
}

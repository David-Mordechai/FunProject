using FunProject.Application.Data.Customers.Command;
using FunProject.Domain.Entities;
using System.Threading.Tasks;

namespace FunProject.Persistence.Customers.Command
{
    public class DeleteCustomerCommand : IDeleteCustomerCommand
    {
        private readonly AppDbContext _appDbContext;

        public DeleteCustomerCommand(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Delete(Customer customer)
        {
            _appDbContext.Customers.Remove(customer);
            await _appDbContext.SaveChangesAsync();
        }
    }
}

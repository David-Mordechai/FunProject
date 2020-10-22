using FunProject.Application.Data.Customers.Command;
using FunProject.Domain.Entities;
using System.Threading.Tasks;

namespace FunProject.Persistence.Customers.Command
{
    public class CreateCustomerCommand : ICreateCustomerCommand
    {
        private readonly AppDbContext _appDbContext;

        public CreateCustomerCommand(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Customer> Create(Customer customer)
        {
            _appDbContext.Customers.Add(customer);
            await _appDbContext.SaveChangesAsync();
            return customer;
        }
    }
}

using FunProject.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunProject.Application.Data.Customers.Query
{
    public interface IAllCustomersQuery
    {
        Task<IList<Customer>> Get();
    }
}

using FunProject.Application.CustomersModule.Dtos;
using FunProject.Application.CustomersModule.Services.Interfaces;
using FunProject.Application.Data.Customers.Command;
using FunProject.Application.Data.Customers.Query;
using FunProject.Domain.Entities;
using FunProject.Domain.Logger;
using FunProject.Domain.Mapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunProject.Application.CustomersModule.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly ILoggerAdapter<CustomersService> _logger;
        private readonly IMapperAdapter _mapperAdapter;

        private readonly ICustomerByIdQuery _customerByIdQuery;
        private readonly IAllCustomersQuery _getAllCustomersQuery;
        private readonly ICreateCustomer _createCustomer;
        private readonly IDeleteCustomer _deleteCustomer;

        public CustomersService(
            ILoggerAdapter<CustomersService> logger,
            IMapperAdapter mapperAdapter,
            ICustomerByIdQuery customerByIdQuery,
            IAllCustomersQuery allCustomersQuery, 
            ICreateCustomer createCustomer,
            IDeleteCustomer deleteCustomer
            )
        {
            _logger = logger;
            _mapperAdapter = mapperAdapter;
            _customerByIdQuery = customerByIdQuery;
            _getAllCustomersQuery = allCustomersQuery;
            _createCustomer = createCustomer;
            _deleteCustomer = deleteCustomer;
        }

        public async Task<IList<CustomerDto>> GetAllCustomers()
        {
            _logger.LogInformation("Method GetAllCustomers was hit...");
            try
            {
                return _mapperAdapter.Map<IList<CustomerDto>>(await _getAllCustomersQuery.Get());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Method GetAllCustomers failed");
                throw;
            }
        }

        public async Task<CustomerDto> GetCustomer(int? id)
        {
            _logger.LogInformation("Method GetCustomer was hit...");
            try
            {
                return _mapperAdapter.Map<CustomerDto>(await _customerByIdQuery.Get(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Method GetCustomer failed");
                throw;
            }
        }

        public async Task CreateCustomer(CustomerDto customer)
        {
            _logger.LogInformation("Method CreateCustomer was hit...");
            try
            {
                await _createCustomer.Create(_mapperAdapter.Map<Customer>(customer));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Method CreateCustomer failed");
                throw;
            }
        }
      
        public async Task DeleteCustomer(int? id)
        {
            _logger.LogInformation("Method DeleteCustomer was hit...");
            try
            {
                var customer = await _customerByIdQuery.Get(id);
                if (customer != null)
                {
                    await _deleteCustomer.Delete(customer);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mehtod DeleteCustomer failed");
                throw;
            }
        }
    }
}

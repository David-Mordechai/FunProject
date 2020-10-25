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
        private readonly ICreateCustomerCommand _createCustomerCommand;
        private readonly IDeleteCustomerCommand _deleteCustomerCommand;

        public CustomersService(
            ILoggerAdapter<CustomersService> logger,
            IMapperAdapter mapperAdapter,
            ICustomerByIdQuery customerByIdQuery,
            IAllCustomersQuery allCustomersQuery, 
            ICreateCustomerCommand createCustomerCommand,
            IDeleteCustomerCommand deleteCustomerCommand
            )
        {
            _logger = logger;
            _mapperAdapter = mapperAdapter;
            _customerByIdQuery = customerByIdQuery;
            _getAllCustomersQuery = allCustomersQuery;
            _createCustomerCommand = createCustomerCommand;
            _deleteCustomerCommand = deleteCustomerCommand;
        }

        public async Task<IList<CustomerDto>> GetAllCustomers()
        {
            _logger.LogInformation("Method GetAllCustomers Invoked.");
            try
            {
                var result = await _getAllCustomersQuery.Get();
                var mappedResult = _mapperAdapter.Map<IList<CustomerDto>>(result);
                return mappedResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Method GetAllCustomers failed.");
                throw;
            }
        }

        public async Task<CustomerDto> GetCustomer(int? id)
        {
            _logger.LogInformation("Method GetCustomer Invoked.");
            try
            {
                return _mapperAdapter.Map<CustomerDto>(await _customerByIdQuery.Get(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Method GetCustomer failed.");
                throw;
            }
        }

        public async Task<CustomerDto> CreateCustomer(CustomerDto customer)
        {
            _logger.LogInformation("Method CreateCustomer Invoked.");
            try
            {
                var createdCustomer = await _createCustomerCommand.Create(_mapperAdapter.Map<Customer>(customer));
                return _mapperAdapter.Map<CustomerDto>(createdCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Method CreateCustomer failed.");
                throw;
            }
        }
      
        public async Task DeleteCustomer(int? id)
        {
            _logger.LogInformation("Method DeleteCustomer Invoked.");
            try
            {
                var customer = await _customerByIdQuery.Get(id);
                if (customer != null)
                {
                    await _deleteCustomerCommand.Delete(customer);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Method DeleteCustomer failed.");
                throw;
            }
        }
    }
}

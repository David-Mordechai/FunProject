using FunProject.Application.CustomersModule.Dtos;
using FunProject.Application.CustomersModule.Services;
using FunProject.Application.Data.Customers.Query;
using FunProject.Domain.Entities;
using FunProject.Domain.Logger;
using FunProject.Domain.Mapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FunProject.Application.Tests.CustomersModule.Serivices.CustomersSerivce
{
    public class GetAllCustomersQueryTests
    {
        private readonly Mock<ILoggerAdapter<CustomersService>> _logger;
        private readonly Mock<IMapperAdapter> _mapper;
        private readonly Mock<IAllCustomersQuery> _allCustomersQuery;

        public GetAllCustomersQueryTests()
        {
            _logger = new Mock<ILoggerAdapter<CustomersService>>();
            _mapper = new Mock<IMapperAdapter>();
            _allCustomersQuery = new Mock<IAllCustomersQuery>();
        }

        [Fact]
        public async Task GetAllCustomers_NoCustomersShouldReturnEmptyListAsync()
        {
            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));
            _mapper.Setup(x => x.Map<IList<CustomerDto>>(new List<Customer>())).Returns(new List<CustomerDto>());
            _allCustomersQuery.Setup(x => x.Get()).ReturnsAsync(new List<Customer>());

            var sut = new CustomersService(_logger.Object, _mapper.Object, null, _allCustomersQuery.Object, null, null);

            var result = await sut.GetAllCustomers();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllCustomers_LogInformationWhenMethodWasCalledAsync()
        {
            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));
            _mapper.Setup(x => x.Map<IList<CustomerDto>>(new List<Customer>())).Returns(new List<CustomerDto>());
            _allCustomersQuery.Setup(x => x.Get()).ReturnsAsync(new List<Customer>());

            var sut = new CustomersService(_logger.Object, _mapper.Object, null, _allCustomersQuery.Object, null, null);

            var result = await sut.GetAllCustomers();

            _logger.Verify(x => x.LogInformation("Method GetAllCustomers was hit..."), Times.Once);
        }

        [Fact]
        public async Task GetAllCustomers_ShouldReturnListOfCustomerDto()
        {
            var customersList = new List<Customer> 
            { 
                new Customer { Id = 1, FirstName = "FirstName1", LastName = "LastName1" }, 
                new Customer { Id = 2, FirstName = "FirstName2", LastName = "LastName2" } 
            };
            var customerDtosList = new List<CustomerDto>
            {
                new CustomerDto { Id = 1, FirstName = "FirstName1", LastName = "LastName1" },
                new CustomerDto { Id = 2, FirstName = "FirstName2", LastName = "LastName2" }
            };

            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));
            _mapper.Setup(x => x.Map<IList<CustomerDto>>(customersList)).Returns(customerDtosList);
            _allCustomersQuery.Setup(x => x.Get()).ReturnsAsync(customersList);

            var sut = new CustomersService(_logger.Object, _mapper.Object, null, _allCustomersQuery.Object, null, null);

            var result = await sut.GetAllCustomers();

            Assert.IsAssignableFrom<IList<CustomerDto>>(result);
        }

        [Fact]
        public async Task GetAllCustomers_AllCustomersQuery_Get_ShouldBeInvoked()
        {
            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));
            _mapper.Setup(x => x.Map<IList<CustomerDto>>(new List<Customer>())).Returns(new List<CustomerDto>());
            _allCustomersQuery.Setup(x => x.Get()).ReturnsAsync(new List<Customer>());

            var sut = new CustomersService(_logger.Object, _mapper.Object, null, _allCustomersQuery.Object, null, null);

            var result = await sut.GetAllCustomers();

            _allCustomersQuery.Verify(x => x.Get(), Times.Once());
        }

        [Fact]
        public async Task GetAllCustomers_AllCustomersQuery_Get_ResultShouldBeMappedFromCustomerListToCustomerDtoList()
        {
            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));
            _mapper.Setup(x => x.Map<IList<CustomerDto>>(new List<Customer>())).Returns(new List<CustomerDto>());
            _allCustomersQuery.Setup(x => x.Get()).ReturnsAsync(new List<Customer>());

            var sut = new CustomersService(_logger.Object, _mapper.Object, null, _allCustomersQuery.Object, null, null);

            var result = await sut.GetAllCustomers();

            _mapper.Verify(x => x.Map<IList<CustomerDto>>(new List<Customer>()), Times.Once());
        }

        [Fact]
        public async Task GetAllCustomers_ShouldReturnCustomerDtoList()
        {
            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));
            _mapper.Setup(x => x.Map<IList<CustomerDto>>(new List<Customer>())).Returns(new List<CustomerDto>());
            _allCustomersQuery.Setup(x => x.Get()).ReturnsAsync(new List<Customer>());

            var sut = new CustomersService(_logger.Object, _mapper.Object, null, _allCustomersQuery.Object, null, null);

            var result = await sut.GetAllCustomers();

            Assert.IsAssignableFrom<IList<CustomerDto>>(result);
        }

        [Fact]
        public async Task GetAllCustomers_OnErrorLogErrorAndThrowException()
        {
            _logger.Setup(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()));
            _mapper.Setup(x => x.Map<IList<CustomerDto>>(new List<Customer>())).Returns(new List<CustomerDto>());
            _allCustomersQuery.Setup(x => x.Get()).Throws(new Exception());

            var sut = new CustomersService(_logger.Object, _mapper.Object, null, _allCustomersQuery.Object, null, null);

            var ex = await Assert.ThrowsAsync<Exception>(() => sut.GetAllCustomers());
            _logger.Verify(x => x.LogError(ex, "Method GetAllCustomers failed"), Times.Once);
        }

    }
}

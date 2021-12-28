using FunProject.Application.CustomersModule.Dtos;
using FunProject.Application.CustomersModule.Services;
using FunProject.Application.Data.Customers.Query;
using FunProject.Domain.Entities;
using FunProject.Domain.Logger;
using FunProject.Domain.Mapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Xunit.Assert;

namespace FunProject.Application.Tests.CustomersModule.Services.CustomersServiceTests
{
    public class GetAllCustomersMethodTests
    {
        private readonly Mock<ILoggerAdapter<CustomersService>> _logger;
        private readonly Mock<IMapperAdapter> _mapper;
        private readonly Mock<IAllCustomersQuery> _allCustomersQuery;

        public GetAllCustomersMethodTests()
        {
            _logger = new Mock<ILoggerAdapter<CustomersService>>();
            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));

            _mapper = new Mock<IMapperAdapter>();
            _mapper.Setup(x => x.Map<IList<CustomerDto>>(new List<Customer>())).Returns(new List<CustomerDto>());

            _allCustomersQuery = new Mock<IAllCustomersQuery>();
            _allCustomersQuery.Setup(x => x.Get()).ReturnsAsync(new List<Customer>());
        }

        [Fact]
        public async Task GetAllCustomers_NoCustomersShouldReturnEmptyListAsync()
        {
            var sut = new CustomersService(_logger.Object, _mapper.Object, null, _allCustomersQuery.Object, null, null);

            var result = await sut.GetAllCustomers();

            Empty(result);
        }

        [Fact]
        public async Task GetAllCustomers_LogInformation_WhenMethodWasCalledAsync()
        {
            var sut = new CustomersService(_logger.Object, _mapper.Object, null, _allCustomersQuery.Object, null, null);

            await sut.GetAllCustomers();

            _logger.Verify(x => x.LogInformation("Method GetAllCustomers Invoked."), Times.Once);
        }

        [Fact]
        public async Task GetAllCustomers_ShouldReturnListOfCustomerDto()
        {
            var customersList = new List<Customer>
            {
                new() { Id = 1, FirstName = "FirstName1", LastName = "LastName1" },
                new() { Id = 2, FirstName = "FirstName2", LastName = "LastName2" }
            };
            var customerDtoList = new List<CustomerDto>
            {
                new() { Id = 1, FirstName = "FirstName1", LastName = "LastName1" },
                new() { Id = 2, FirstName = "FirstName2", LastName = "LastName2" }
            };

            _mapper.Setup(x => x.Map<IList<CustomerDto>>(customersList)).Returns(customerDtoList);
            _allCustomersQuery.Setup(x => x.Get()).ReturnsAsync(customersList);

            var sut = new CustomersService(_logger.Object, _mapper.Object, null, _allCustomersQuery.Object, null, null);

            var result = await sut.GetAllCustomers();

            IsAssignableFrom<IList<CustomerDto>>(result);
            Equal(customerDtoList.First().Id, customersList.First().Id);
            Equal(customerDtoList.First().FirstName, customersList.First().FirstName);
            Equal(customerDtoList.First().LastName, customersList.First().LastName);
        }

        [Fact]
        public async Task GetAllCustomers_AllCustomersQuery_Get_ShouldBeInvoked()
        {
            var sut = new CustomersService(_logger.Object, _mapper.Object, null, _allCustomersQuery.Object, null, null);

            await sut.GetAllCustomers();

            _allCustomersQuery.Verify(x => x.Get(), Times.Once());
        }

        [Fact]
        public async Task GetAllCustomers_AllCustomersQuery_Get_ResultShouldBeMappedFromCustomerListToCustomerDtoList()
        {
            var sut = new CustomersService(_logger.Object, _mapper.Object, null, _allCustomersQuery.Object, null, null);

            await sut.GetAllCustomers();

            _mapper.Verify(x => x.Map<IList<CustomerDto>>(new List<Customer>()), Times.Once());
        }

        [Fact]
        public async Task GetAllCustomers_OnError_LogError_And_ThrowException()
        {
            _logger.Setup(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()));
            _allCustomersQuery.Setup(x => x.Get()).Throws(new Exception());

            var sut = new CustomersService(_logger.Object, _mapper.Object, null, _allCustomersQuery.Object, null, null);

            var ex = await ThrowsAsync<Exception>(() => sut.GetAllCustomers());
            _logger.Verify(x => x.LogError(ex, "Method GetAllCustomers failed."), Times.Once);
        }
    }
}
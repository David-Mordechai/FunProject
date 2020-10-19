using FunProject.Application.CustomersModule.Dtos;
using FunProject.Application.CustomersModule.Services;
using FunProject.Application.Data.Customers.Query;
using FunProject.Domain.Entities;
using FunProject.Domain.Logger;
using FunProject.Domain.Mapper;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FunProject.Application.Tests.CustomersModule.Serivices
{
    public class CustomersServiceTests
    {
        private readonly Mock<ILoggerAdapter<CustomersService>> _logger;
        private readonly Mock<IMapperAdapter> _mapper;
        public CustomersServiceTests()
        {
            _logger = new Mock<ILoggerAdapter<CustomersService>>();
            _mapper = new Mock<IMapperAdapter>();
        }

        [Fact]
        public async Task CustomersSerivce_GetAllCustomers_NoCustomersShouldReturnEmptyListAsync()
        {
            IList<Customer> customers = new List<Customer>();

            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));
            _mapper.Setup(x => x.Map<IList<CustomerDto>>(customers)).Returns(new List<CustomerDto>());

            var allCustomersQuery = new Mock<IAllCustomers>();
            allCustomersQuery.Setup(x => x.Get()).ReturnsAsync(customers);

            var customersService = new CustomersService(null, allCustomersQuery.Object, null, null, _mapper.Object, _logger.Object);

            var result = await customersService.GetAllCustomers();

            _logger.Verify(x => x.LogInformation("Method GetAllCustomers was hit..."), Times.Once);
            _mapper.Verify(x => x.Map<IList<CustomerDto>>(customers), Times.Once());
            allCustomersQuery.Verify(x => x.Get(), Times.Once());

            Assert.Empty(result);
            Assert.IsAssignableFrom<IList<CustomerDto>>(result);
        }
    }
}

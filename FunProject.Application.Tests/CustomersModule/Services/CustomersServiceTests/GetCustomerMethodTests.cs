using FunProject.Application.CustomersModule.Dtos;
using FunProject.Application.CustomersModule.Services;
using FunProject.Application.Data.Customers.Query;
using FunProject.Domain.Entities;
using FunProject.Domain.Logger;
using FunProject.Domain.Mapper;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using static Xunit.Assert;

namespace FunProject.Application.Tests.CustomersModule.Services.CustomersServiceTests
{
    public class GetCustomerMethodTests
    {
        private readonly Mock<ILoggerAdapter<CustomersService>> _logger;
        private readonly Mock<IMapperAdapter> _mapper;
        private readonly Mock<ICustomerByIdQuery> _customerByIdQuery;
        private readonly Customer _customer = new() { Id = 1, FirstName = "FirstName1", LastName = "LastName1" };
        private readonly CustomerDto _customerDto = new() { Id = 1, FirstName = "FirstName1", LastName = "LastName1" };

        public GetCustomerMethodTests()
        {
            _logger = new Mock<ILoggerAdapter<CustomersService>>();
            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));

            _mapper = new Mock<IMapperAdapter>();
            _mapper.Setup(x => x.Map<CustomerDto>(_customer)).Returns(_customerDto);

            _customerByIdQuery = new Mock<ICustomerByIdQuery>();
            _customerByIdQuery.Setup(x => x.Get(1)).ReturnsAsync(_customer);
        }

        [Fact]
        public async Task GetCustomer_LogInformation_WhenMethodWasCalledAsync()
        {
            var sut = new CustomersService(_logger.Object, _mapper.Object, _customerByIdQuery.Object, null, null, null);

            await sut.GetCustomer(1);

            _logger.Verify(x => x.LogInformation("Method GetCustomer Invoked."), Times.Once);
        }

        [Fact]
        public async Task GetCustomer_CustomerNotFound_ReturnNullAsync()
        {
            _customerByIdQuery.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(() => null);

            var sut = new CustomersService(_logger.Object, _mapper.Object, _customerByIdQuery.Object, null, null, null);

            var result = await sut.GetCustomer(1);

            Null(result);
        }

        [Fact]
        public async Task GetCustomer_ShouldReturnCustomerDto()
        {
            _mapper.Setup(x => x.Map<CustomerDto>(_customer)).Returns(_customerDto);
            _customerByIdQuery.Setup(x => x.Get(1)).ReturnsAsync(_customer);

            var sut = new CustomersService(_logger.Object, _mapper.Object, _customerByIdQuery.Object, null, null, null);

            var result = await sut.GetCustomer(1);

            IsAssignableFrom<CustomerDto>(result);
            Equal(_customerDto.Id, _customer.Id);
            Equal(_customerDto.FirstName, _customer.FirstName);
            Equal(_customerDto.LastName, _customer.LastName);
        }

        [Fact]
        public async Task GetCustomer_CustomerByIdQuery_Get_ShouldBeInvoked()
        {
            var sut = new CustomersService(_logger.Object, _mapper.Object, _customerByIdQuery.Object, null, null, null);

            await sut.GetCustomer(1);

            _customerByIdQuery.Verify(x => x.Get(1), Times.Once());
        }

        [Fact]
        public async Task GetCustomer_CustomerByIdQuery_Get_ResultShouldBeMappedFromCustomerToCustomerDto()
        {
            var sut = new CustomersService(_logger.Object, _mapper.Object, _customerByIdQuery.Object, null, null, null);

            await sut.GetCustomer(1);

            _mapper.Verify(x => x.Map<CustomerDto>(_customer), Times.Once());
        }

        [Fact]
        public async Task GetCustomer_OnError_LogError_And_ThrowException()
        {
            _logger.Setup(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()));
            _customerByIdQuery.Setup(x => x.Get(1)).Throws(new Exception());

            var sut = new CustomersService(_logger.Object, _mapper.Object, _customerByIdQuery.Object, null, null, null);

            var ex = await ThrowsAsync<Exception>(() => sut.GetCustomer(1));
            _logger.Verify(x => x.LogError(ex, "Method GetCustomer failed."), Times.Once);
        }
    }
}

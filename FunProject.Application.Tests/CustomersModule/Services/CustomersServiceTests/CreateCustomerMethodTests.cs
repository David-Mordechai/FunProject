using FunProject.Application.CustomersModule.Dtos;
using FunProject.Application.CustomersModule.Services;
using FunProject.Application.Data.Customers.Command;
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
    public class CreateCustomerMethodTests
    {
        private readonly Customer _customer = new() { Id = 0, FirstName = "FirstName1", LastName = "LastName1" };
        private readonly CustomerDto _customerDto = new() { Id = 0, FirstName = "FirstName1", LastName = "LastName1" };

        private readonly Customer _createdCustomer = new() { Id = 1, FirstName = "FirstName1", LastName = "LastName1" };
        private readonly CustomerDto _createdCustomerDto = new() { Id = 1, FirstName = "FirstName1", LastName = "LastName1" };

        private readonly Mock<ILoggerAdapter<CustomersService>> _logger;
        private readonly Mock<IMapperAdapter> _mapper;
        private readonly Mock<ICreateCustomerCommand> _createCustomerCommand;

        public CreateCustomerMethodTests()
        {
            _logger = new Mock<ILoggerAdapter<CustomersService>>();
            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));

            _mapper = new Mock<IMapperAdapter>();
            _mapper.Setup(x => x.Map<Customer>(_customerDto)).Returns(_customer);

            _createCustomerCommand = new Mock<ICreateCustomerCommand>();
            _createCustomerCommand.Setup(x => x.Create(_customer)).ReturnsAsync(_createdCustomer);
        }

        [Fact]
        public async Task CreateCustomer_LogInformation_WhenMethodWasCalledAsync()
        {
            var sut = new CustomersService(_logger.Object, _mapper.Object, null, null, _createCustomerCommand.Object, null);

            await sut.CreateCustomer(_customerDto);

            _logger.Verify(x => x.LogInformation("Method CreateCustomer Invoked."), Times.Once);
        }

        [Fact]
        public async Task CreateCustomer_CreateCommand_Parameter_MapCustomerDtoToCustomerEntity()
        {
            var sut = new CustomersService(_logger.Object, _mapper.Object, null, null, _createCustomerCommand.Object, null);

            await sut.CreateCustomer(_customerDto);

            _mapper.Verify(x => x.Map<Customer>(_customerDto), Times.Once());
        }

        [Fact]
        public async Task CreateCustomer_CustomerCreated_Successfully()
        {
            _mapper.Setup(x => x.Map<CustomerDto>(_createdCustomer)).Returns(_createdCustomerDto);

            var sut = new CustomersService(_logger.Object, _mapper.Object, null, null, _createCustomerCommand.Object, null);

            var result = await sut.CreateCustomer(_customerDto);

            _mapper.Verify(x => x.Map<CustomerDto>(_createdCustomer), Times.Once());
            Equal(_createdCustomerDto.Id, result.Id);
        }

        [Fact]
        public async Task CreateCustomer_OnError_LogError_And_ThrowException()
        {
            _logger.Setup(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()));
            _createCustomerCommand.Setup(x => x.Create(_customer)).Throws(new Exception());

            var sut = new CustomersService(_logger.Object, _mapper.Object, null, null, _createCustomerCommand.Object, null);

            var ex = await ThrowsAsync<Exception>(() => sut.CreateCustomer(_customerDto));
            _logger.Verify(x => x.LogError(ex, "Method CreateCustomer failed."), Times.Once);
        }
    }
}

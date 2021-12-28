using FunProject.Application.CustomersModule.Services;
using FunProject.Application.Data.Customers.Command;
using FunProject.Application.Data.Customers.Query;
using FunProject.Domain.Entities;
using FunProject.Domain.Logger;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using static Xunit.Assert;

namespace FunProject.Application.Tests.CustomersModule.Services.CustomersServiceTests
{
    public class DeleteCustomerMethodTests
    {
        private readonly Customer _customer = new() { Id = 1, FirstName = "FirstName1", LastName = "LastName1" };
        private readonly Mock<ILoggerAdapter<CustomersService>> _logger;
        private readonly Mock<IDeleteCustomerCommand> _deleteCustomerCommand;
        private readonly Mock<ICustomerByIdQuery> _customerByIdQuery;

        public DeleteCustomerMethodTests()
        {
            _logger = new Mock<ILoggerAdapter<CustomersService>>();
            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));

            _customerByIdQuery = new Mock<ICustomerByIdQuery>();
            _customerByIdQuery.Setup(x => x.Get(1)).ReturnsAsync(_customer);

            _deleteCustomerCommand = new Mock<IDeleteCustomerCommand>();
            _deleteCustomerCommand.Setup(x => x.Delete(_customer));
        }

        [Fact]
        public async Task DeleteCustomer_LogInformation_WhenMethodWasCalledAsync()
        {
            var sut = new CustomersService(_logger.Object, null, _customerByIdQuery.Object, null, null, _deleteCustomerCommand.Object);

            await sut.DeleteCustomer(1);

            _logger.Verify(x => x.LogInformation("Method DeleteCustomer Invoked."), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomer_GetCustomerById_FindCustomerByIdForDeletion()
        {
            var sut = new CustomersService(_logger.Object, null, _customerByIdQuery.Object, null, null, _deleteCustomerCommand.Object);

            await sut.DeleteCustomer(1);

            _customerByIdQuery.Verify(x => x.Get(1), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomer_WhenCustomerNotNotFound_DoNotDeleteCustomer()
        {
            var sut = new CustomersService(_logger.Object, null, _customerByIdQuery.Object, null, null, _deleteCustomerCommand.Object);
            _customerByIdQuery.Setup(x => x.Get(1)).ReturnsAsync(() => null);

            await sut.DeleteCustomer(1);

            _deleteCustomerCommand.Verify(x => x.Delete(null), Times.Never);
        }

        [Fact]
        public async Task DeleteCustomer_WhenCustomerFound_DeleteCustomer()
        {
            var sut = new CustomersService(_logger.Object, null, _customerByIdQuery.Object, null, null, _deleteCustomerCommand.Object);

            await sut.DeleteCustomer(1);

            _deleteCustomerCommand.Verify(x => x.Delete(_customer), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomer_CustomerByIdQuery_OnError_LogError_And_ThrowException()
        {
            _logger.Setup(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()));
            _customerByIdQuery.Setup(x => x.Get(1)).Throws(new Exception());

            var sut = new CustomersService(_logger.Object, null, _customerByIdQuery.Object, null, null, _deleteCustomerCommand.Object);

            var ex = await ThrowsAsync<Exception>(() => sut.DeleteCustomer(1));
            _logger.Verify(x => x.LogError(ex, "Method DeleteCustomer failed."), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomer_DeleteCustomerCommand_OnError_LogError_And_ThrowException()
        {
            _logger.Setup(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()));
            _deleteCustomerCommand.Setup(x => x.Delete(_customer)).Throws(new Exception());

            var sut = new CustomersService(_logger.Object, null, _customerByIdQuery.Object, null, null, _deleteCustomerCommand.Object);

            var ex = await ThrowsAsync<Exception>(() => sut.DeleteCustomer(1));
            _logger.Verify(x => x.LogError(ex, "Method DeleteCustomer failed."), Times.Once);
        }
    }
}

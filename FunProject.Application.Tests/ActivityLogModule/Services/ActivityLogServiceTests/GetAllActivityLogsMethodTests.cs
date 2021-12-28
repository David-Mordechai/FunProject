using FunProject.Domain.Logger;
using FunProject.Application.ActivityLogModule.Services;
using Moq;
using FunProject.Domain.Mapper;
using FunProject.Application.Data.ActivityLogs.Query;
using FunProject.Domain.Entities;
using System.Collections.Generic;
using FunProject.Application.ActivityLogModule.Dtos;
using System;
using FunProject.Domain.Enums;
using System.Threading.Tasks;
using Xunit;
using static Xunit.Assert;

namespace FunProject.Application.Tests.ActivityLogModule.Services.ActivityLogServiceTests
{
    public class GetAllActivityLogsMethodTests
    {
        private readonly Mock<ILoggerAdapter<ActivityLogService>> _logger;
        private readonly Mock<IMapperAdapter> _mapper;
        private readonly Mock<IAllActivityLogsQuery> _allActivityLogsQuery;

        private static readonly DateTime ActivityDate = new(2020, 01, 15);

        private  readonly IList<ActivityLog> _activityLogs = new List<ActivityLog>
        {
            new()
            {
                Id = 1, 
                CustomerId= 1, 
                FirstName = "FirstName1", 
                LastName = "LastName1", 
                ActivityDate = ActivityDate, 
                ActionType = ActionType.Create 
            }
        };

        private readonly IList<ActivityLogDto> _activityLogsDtoList = new List<ActivityLogDto>
        {
            new()
            {
                Id = 1,
                CustomerId= 1,
                FirstName = "FirstName1",
                LastName = "LastName1",
                ActivityDate = ActivityDate,
                ActionType = ActionType.Create
            }
        };

        public GetAllActivityLogsMethodTests()
        {
            _logger = new Mock<ILoggerAdapter<ActivityLogService>>();
            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));

            _mapper = new Mock<IMapperAdapter>();
            _mapper.Setup(x => x.Map<IList<ActivityLogDto>>(_activityLogs)).Returns(_activityLogsDtoList);

            _allActivityLogsQuery = new Mock<IAllActivityLogsQuery>();
            _allActivityLogsQuery.Setup(x => x.Get()).ReturnsAsync(_activityLogs);
        }

        [Fact]
        public async Task GetAllActivityLogs_LogInformation_WhenMethodWasCalled_Async()
        {
            var sut = new ActivityLogService(_logger.Object, _mapper.Object, _allActivityLogsQuery.Object);

            await sut.GetAllActivityLogs();

            _logger.Verify(x => x.LogInformation("Method GetAllActivityLogs Invoked."), Times.Once,
                "Information log was not logged.");
        }

        [Fact]
        public async Task GetAllActivityLogs_NoActivityLogsShouldReturnEmptyList_Async()
        {
            _allActivityLogsQuery.Setup(x => x.Get()).ReturnsAsync(new List<ActivityLog>());
            _mapper.Setup(x => x.Map<IList<ActivityLogDto>>(new List<ActivityLog>())).Returns(new List<ActivityLogDto>());
            
            var sut = new ActivityLogService(_logger.Object, _mapper.Object, _allActivityLogsQuery.Object);
            var result = await sut.GetAllActivityLogs();

            Empty(result);
        }
        
        [Fact]
        public async Task GetAllActivityLogs_ShouldInvoke_AllActivityLogsQuery_Get_Async()
        {
            var sut = new ActivityLogService(_logger.Object, _mapper.Object, _allActivityLogsQuery.Object);

            await sut.GetAllActivityLogs();

            _allActivityLogsQuery.Verify(x => x.Get(), Times.Once, 
                "AllActivityLogsQuery.Get Method should be Invoked.");
        }

        [Fact]
        public async Task GetAllActivityLogs_AllActivityLogsQuery_Get_Result_MapTo_ActivityLogDtoList_Async()
        {
            var sut = new ActivityLogService(_logger.Object, _mapper.Object, _allActivityLogsQuery.Object);

            await sut.GetAllActivityLogs();

            _mapper.Verify(x => x.Map<IList<ActivityLogDto>>(_activityLogs), Times.Once(), 
                "AllActivityLogsQuery.Get Method result should be mapped from entity to dto list.");
        }

        [Fact]
        public async Task GetAllActivityLogs_ShouldReturnListOfActivityLogs_Async()
        {
            var sut = new ActivityLogService(_logger.Object, _mapper.Object, _allActivityLogsQuery.Object);
            
            var result = await sut.GetAllActivityLogs();

            Equal(_activityLogsDtoList, result);
        }

        [Fact]
        public async Task GetAllActivityLogs_AllActivityLogsQuery_OnError_LogError_And_ThrowException()
        {
            _logger.Setup(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()));
            _allActivityLogsQuery.Setup(x => x.Get()).Throws(new Exception());

            var sut = new ActivityLogService(_logger.Object, _mapper.Object, _allActivityLogsQuery.Object);

            var ex = await ThrowsAsync<Exception>(() => sut.GetAllActivityLogs());
            _logger.Verify(x => x.LogError(ex, "Method GetAllActivityLogs failed."), Times.Once,
                "Error was not logged.");
        }
    }
}

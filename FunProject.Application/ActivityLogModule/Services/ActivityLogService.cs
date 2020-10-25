using FunProject.Application.ActivityLogModule.Dtos;
using FunProject.Application.ActivityLogModule.Services.Interfaces;
using FunProject.Application.Data.ActivityLogs.Query;
using FunProject.Domain.Logger;
using FunProject.Domain.Mapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunProject.Application.ActivityLogModule.Services
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly ILoggerAdapter<ActivityLogService> _logger;
        private readonly IMapperAdapter _mapperAdapter;
        private readonly IAllActivityLogsQuery _allActivityLogsQuery;

        public ActivityLogService(
            ILoggerAdapter<ActivityLogService> logger,
            IMapperAdapter mapperAdapter, 
            IAllActivityLogsQuery allActivityLogsQuery)
        {
            _logger = logger;
            _mapperAdapter = mapperAdapter;
            _allActivityLogsQuery = allActivityLogsQuery;
        }

        public async Task<IList<ActivityLogDto>> GetAllActivityLogs()
        {
            _logger.LogInformation("Method GetAllActivityLogs Invoked.");
            try
            {
                return _mapperAdapter.Map<IList<ActivityLogDto>>(await _allActivityLogsQuery.Get());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Method GetAllActivityLogs failed.");
                throw;
            }
        }
    }
}

using FunProject.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunProject.Application.Data.ActivityLogs.Query
{
    public interface IAllActivityLogsQuery
    {
        Task<IList<ActivityLog>> Get();
    }
}

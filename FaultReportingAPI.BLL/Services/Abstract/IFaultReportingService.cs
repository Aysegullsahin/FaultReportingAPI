using FaultReportingAPI.BLL.Services.Abstract.Base;
using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Enums;
using FaultReportingAPI.Core.Models;
using FaultReportingAPI.Core.Models.Base;

namespace FaultReportingAPI.BLL.Services.Abstract
{
    public interface IFaultReportingService : IBaseService<FaultReporting>
    {
        Task<DataResult<CoreBaseEntity>> AddAsync(FaultReportingDto_AddRequest entityDto);
        Task<DataResult<CoreBaseEntity>> UpdateAsync(FaultReportingDto_UpdateRequest entityDto, Dictionary<string, object>? changedFields = null);

        Task<DataResult<CoreBaseEntity>> ChangeStatusAsync(long id, StatusEnum newStatus);
        Task<DataResult<CoreBaseEntity>> GetFaultReportingListAsync(StatusEnum? status, PriorityLevelEnum? priorityLevel, string? location, FaultReportSortByEnum? sortBy, bool isDescending, int pageNumber = 1, int pageSize = 10);
    }
}

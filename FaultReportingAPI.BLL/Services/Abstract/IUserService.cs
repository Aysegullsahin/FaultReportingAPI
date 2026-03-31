using FaultReportingAPI.BLL.Services.Abstract.Base;
using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Models;
using FaultReportingAPI.Core.Models.Base;

namespace FaultReportingAPI.BLL.Services.Abstract
{
    public interface IUserService : IBaseService<User>
    {
        Task<DataResult<CoreBaseEntity>> UpdateAsync(UserDto_Request entityDto, Dictionary<string, object>? changedFields = null);
        Task<DataResult<UserDto_Token>> LoginUserAsync(string email, string password);
        Task<DataResult<UserDto_Token>> RegisterAsync(UserDto_Request entityDto);

    }
}

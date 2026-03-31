
using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Models;

namespace FaultReportingAPI.BLL.Mapper.Abstract
{
    public interface IUserMapper
    {
        UserDto_S ToDto_S(User user);
        UserDto_L ToDto_L(User user);
        User ToEntity(UserDto_Request dto);
    }
}

using FaultReportingAPI.BLL.Mapper.Abstract;
using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Models;
using Riok.Mapperly.Abstractions;

namespace FaultReportingAPI.BLL.Mapper
{
    [Mapper]
    public partial class UserMapper : IUserMapper
    {
        public partial UserDto_S ToDto_S(User user);
        public partial UserDto_L ToDto_L(User user);
        public partial User ToEntity(UserDto_Request dto);
    }
}

using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Models;

namespace FaultReportingAPI.BLL.Mapper.Abstract
{
    public interface IFaultReportingMapper
    {
        FaultReportingDto_S ToDto_S(FaultReporting? entity);
        FaultReportingDto_L ToDto_L(FaultReporting? entity);
        FaultReporting ToAddEntity(FaultReportingDto_AddRequest dto);
        FaultReporting ToUpdateEntity(FaultReportingDto_UpdateRequest dto);
    }
}

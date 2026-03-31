using FaultReportingAPI.BLL.Mapper.Abstract;
using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Models;
using Riok.Mapperly.Abstractions;

namespace FaultReportingAPI.BLL.Mapper
{
    [Mapper]
    public partial class FaultReportingMapper : IFaultReportingMapper
    {
        public partial FaultReportingDto_S ToDto_S(FaultReporting? entity);
        public partial FaultReportingDto_L ToDto_L(FaultReporting? entity);
        public partial FaultReporting ToAddEntity(FaultReportingDto_AddRequest dto);
        public partial FaultReporting ToUpdateEntity(FaultReportingDto_UpdateRequest dto);

    }
}

using FaultReportingAPI.Core.Enums;
using FaultReportingAPI.Core.Models.Base;

namespace FaultReportingAPI.Core.Dto
{
    #region REQUEST
    public class FaultReportingDto_AddRequest 
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Location { get; set; }
        public required PriorityLevelEnum Priority { get; set; }
    }
    public class FaultReportingDto_UpdateRequest
    {
        public required long Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Location { get; set; }
        public required PriorityLevelEnum Priority { get; set; }
    }

    #endregion

    #region RESPONSE
    public class FaultReportingDto_S : CoreBaseEntity
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }

        public PriorityLevelEnum Priority { get; set; }
        public StatusEnum Status { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string? CreatedUserName { get; set; }
        public string? CreatedUserSurname { get; set; }
        public string? UpdatedUserName { get; set; }
        public string? UpdatetedUserSurname { get; set; }
    }

    public class FaultReportingDto_L : FaultReportingDto_S
    {
    }
    #endregion


}

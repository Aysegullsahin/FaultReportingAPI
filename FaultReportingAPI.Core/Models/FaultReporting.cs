using FaultReportingAPI.Core.Enums;
using FaultReportingAPI.Core.Models.Base;

namespace FaultReportingAPI.Core.Models
{
    public class FaultReporting : BaseEntity
    {
        public string? Title { get; set; }

        public string? Description { get; set; } 

        // Free text location (City/District/Neighborhood etc.)
        public string? Location { get; set; } 

        public PriorityLevelEnum Priority { get; set; }

        public StatusEnum Status { get; set; }

    }
}

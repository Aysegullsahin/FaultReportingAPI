using FaultReportingAPI.Core.Enums;
using FaultReportingAPI.Core.Models.Base;

namespace FaultReportingAPI.Core.Models
{
    public class User : BaseEntity
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Password { get; set; }
        public required string Email { get; set; }
        public RoleEnum? Role { get; set; }
    }
}

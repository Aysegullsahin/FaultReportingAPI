using FaultReportingAPI.Core.Enums;
using FaultReportingAPI.Core.Models.Base;

namespace FaultReportingAPI.Core.Dto
{

    #region REQUEST
    public class UserDto_Request
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
    }
    #endregion

    #region RESPONSE
    public class UserDto_S : CoreBaseEntity
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
    }
    public class UserDto_L : UserDto_S
    {

    }


    public class UserDto_LoginResponse : CoreBaseEntity
    {
        public required long Id { get; set; }
        public required string HashPassword { get; set; }
        public required string Email { get; set; }
        public RoleEnum? Role { get; set; }
    }
    public class UserDto_Token : CoreBaseEntity
    {
        public long? Id { get; set; }
        public required string JWTToken { get; set; }
    }
    
    #endregion

}

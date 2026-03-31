using FaultReportingAPI.Core.Enums;
using System.Security.Claims;

namespace FaultReportingAPI.BLL.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static long GetUserId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst("UserId");
            return claim == null ? throw new UnauthorizedAccessException("UserId claim missing") : long.Parse(claim.Value);
        }
        public static RoleEnum GetRole(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.Role);
            return claim == null ? throw new UnauthorizedAccessException("Role claim missing") : Enum.Parse<RoleEnum>(claim.Value);
        }
    }
}

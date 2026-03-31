using FaultReportingAPI.Core.Enums;
using Microsoft.AspNetCore.Authorization;

namespace FaultReportingAPI.Functions
{
    public class TokenAuthorizeAttribute : AuthorizeAttribute
    {
        public TokenAuthorizeAttribute(params RoleEnum[] roles)
        {
            Policy = RolePolicyHelper.BuildPolicyName(roles);
        }
    }
}

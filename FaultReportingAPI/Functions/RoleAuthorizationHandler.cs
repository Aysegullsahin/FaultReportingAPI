using FaultReportingAPI.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FaultReportingAPI.Functions
{
    public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (!context.User.Identity?.IsAuthenticated ?? true)
                return Task.CompletedTask;

            var roleClaim = context.User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleClaim == null)
                return Task.CompletedTask;

            if (!Enum.TryParse<RoleEnum>(roleClaim, out var userRole))
                return Task.CompletedTask;

            if (requirement.Roles.Contains(userRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class RoleRequirement(RoleEnum[] roles) : IAuthorizationRequirement
    {
        public RoleEnum[] Roles { get; } = roles;
    }
    public static class RolePolicyHelper
    {
        public static string BuildPolicyName(RoleEnum[] roles)
        {
            return "ROLE_POLICY_" + string.Join("_", roles);
        }
    }
}

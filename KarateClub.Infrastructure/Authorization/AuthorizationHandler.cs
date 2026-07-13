using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace KarateClub.Infrastructure.Authorization
{
    public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            
            if (!context.User.Identity?.IsAuthenticated ?? true)
                return Task.CompletedTask;

            bool isSuperAdmin = bool.TryParse(context.User.Claims.FirstOrDefault(c => c.Type == "IsSuperAdmin")!.Value, out bool result) && result;

            if (isSuperAdmin)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (context.User.HasClaim("permission", requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

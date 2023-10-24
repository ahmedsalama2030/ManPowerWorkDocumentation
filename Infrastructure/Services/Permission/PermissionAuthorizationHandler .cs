using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services.Permission
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IRolePermission _iRolePermission;

        public PermissionAuthorizationHandler(
    IServiceScopeFactory serviceScopeFactory,
    IRolePermission IRolePermission
        )
        {
            _serviceScopeFactory = serviceScopeFactory;
            _iRolePermission = IRolePermission;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
            return;
              context.Succeed(requirement);
                return;
         }
    }
}
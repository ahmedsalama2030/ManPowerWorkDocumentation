using System.Net;
using System.Security.Claims;
using Core.Entities.Management;
using Core.Exceptions;
using Core.Interfaces.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Permission;
public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
    private readonly IHttpContextAccessor _accssor;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IStringLocalizerCustom _localizer;

    public PermissionPolicyProvider(
       IOptions<AuthorizationOptions> options,
       IHttpContextAccessor accssor,
       IServiceScopeFactory serviceScopeFactory,
        IStringLocalizerCustom localizer)
    {

        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        _accssor = accssor;
        _serviceScopeFactory = serviceScopeFactory;
        _localizer = localizer;
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

    public async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        var user = _accssor.HttpContext.User;
        if (string.IsNullOrEmpty(policyName) || user == null)
            throw new ExceptionCommonReponse("forbidden", 403);

        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var _iRolePermission = scope.ServiceProvider.GetRequiredService<IRolePermission>();
            var claims = _iRolePermission.getPermisionForUserAsync(user.Claims).Result;
            var permissionss = claims.Where(x => x.Type.ToLower() == "permission" &&
                                                           x.Value.ToLower() == policyName.ToLower());
            var policy = new AuthorizationPolicyBuilder();

            if (permissionss.Any())
            {
                policy.AddRequirements(new PermissionRequirement(policyName));
                return await Task.FromResult(policy.Build());
            }
            throw new ExceptionCommonReponse("forbidden", 403);
        }
    }

    public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
}

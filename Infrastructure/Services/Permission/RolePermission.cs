using System.Security.Claims;
using Core.Entities.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Permission;
    public class RolePermission : IRolePermission
    {
        private List<Claim> rolesUser = new();
        private readonly RoleManager<Role> _roleManager;
        public RolePermission(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<List<Claim>> getPermisionForUserAsync(AuthorizationHandlerContext context)
        {
            if (rolesUser.Any())
                return rolesUser;
            var roles = context.User.Claims.Where(a => a.Type == ClaimTypes.Role).ToList();
            foreach (var item in roles)
            {
                var role = await _roleManager.FindByNameAsync(item.Value.ToString());
                var claimsXX = await _roleManager.GetClaimsAsync(role);
                rolesUser.AddRange(claimsXX);
            }
            return rolesUser;
        }
       public async Task<List<Claim>> getPermisionForUserAsync(IEnumerable<Claim> claims)
        {
            if (rolesUser.Any())
                return rolesUser;
            var roles = claims.Where(a => a.Type == ClaimTypes.Role).ToList();
            foreach (var item in roles)
            {
                var role = await _roleManager.FindByNameAsync(item.Value.ToString());
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                rolesUser.AddRange(roleClaims);
            }
            return rolesUser;
        }
    
}


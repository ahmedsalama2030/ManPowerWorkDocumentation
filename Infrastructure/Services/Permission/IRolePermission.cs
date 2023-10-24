using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Services.Permission;
    public interface IRolePermission
    {
        Task<List<Claim>> getPermisionForUserAsync(AuthorizationHandlerContext context);
        Task<List<Claim>> getPermisionForUserAsync(IEnumerable<Claim> claims);
}

using Application.Common.Pagination;
using Application.Dtos.Auth.ClaimApp;
using Application.IBusiness.Common;
using Core.Entities.Management;

namespace Application.IBusiness.Management;
public interface IClaimAppBusiness : IEntitiesBusinessCommon<
ClaimApp,
ClaimAppGetDto,
ClaimAppEditDto,
ClaimAppEditDto,
PaginationParam>
{
   Task<HashSet<ClaimsSelect>> GetClaimsOrg(int roleId);
    Task<HashSet<ClaimsSelect>> GetUserClaimsOrg(int userId);
    Task<HashSet<ClaimsSelect>> GetUserClaimsForRoleOrg(Guid userId);
 }

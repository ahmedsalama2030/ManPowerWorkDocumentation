using Application.Common.Pagination;
using Application.Dtos.Auth.RoleClaim;
using Application.Dtos.Message;
using Microsoft.AspNetCore.Http;
namespace Application.IBusiness.Management;
public interface IRoleClaimBusiness
{

    Task<IEnumerable<RoleClaimGetDto>> Get(HttpResponse Response, PaginationParam paginationParam);
    Task<IEnumerable<RoleClaimGetDto>> GetAll();
    Task<RepositoryMessage> GetRole(Guid id);
    Task<RepositoryMessage> Register(RoleClaimRegisterDto RoleRegister);
    Task<RepositoryMessage> Edit(Guid id, RoleClaimEditDto RoleRegister);
    Task<RepositoryMessage> Delete(Guid id);
    Task<RepositoryMessage> DeleteRange(params Guid[] roles);
}
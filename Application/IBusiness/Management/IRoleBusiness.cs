using Application.Common.Pagination;
using Application.Dtos.roles;
using Core.Common.Dto;
using Microsoft.AspNetCore.Http;
using Application.Dtos.Message;
using Application.Dtos.Auth.roles;

namespace Application.IBusiness.Management;
public interface IRoleBusiness
{
    Task<IEnumerable<RoleListDto>> Get(HttpResponse Response, PaginationParam paginationParam);
    Task<IEnumerable<RoleListDto>> GetAll();
    Task<RepositoryMessage> GetRole(int id);
    Task Register(RoleRegisterDto RoleRegister);
    Task Edit(int id, RoleRegisterDto RoleRegister);
    Task Delete(int id);
    Task DeleteRange(params int[] roles);
    Task<List<BaseListDto>> GetAllList();

   Task UpdatePermission( RolePermisionEditDto roleClaimEdit);



}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Pagination;
using Application.Dtos.roles;
using Application.Dtos.Users;
using Core.Common.Dto;
using Microsoft.AspNetCore.Http;
using Application.Dtos.Message;
using Application.Dtos.Auth.Users;
using Application.Dtos.Auth.roles;

namespace Application.IBusiness.Management;
public interface IUserBusiness
{
  Task<IEnumerable<UserListDto>> Get(HttpResponse Response, UserParam paginationParam);
  Task<IEnumerable<UserListDto>> GetTeamWork(HttpResponse Response, UserParam paginationParam);
  Task<IEnumerable<UserListDto>> GetUsersForRole(HttpResponse Response, int userId, string role, UserParam paginationParam);
  Task<IEnumerable<RoleListDto>> GetRoleUser(int id);
  Task TeamWorkEdit(int id, UserEditDto userEdit);
  Task<RepositoryMessage> GetUser(int id);
  Task AssignRoles(int userId, params string[] roles);
  Task ChangePassword(int id, PaaswordDto newpassword);
  Task DeleteById(int id);
  Task<UserAnalysis> UsersAnalysis();

  Task Edit(int id, UserEditDto userEdit);
  Task DeleteRange(int[] users);
  Task DeleteHard(int[] users);
  Task<List<BaseListDto>> GetAllList();
  Task<RepositoryMessage> GetByClient(Guid id);
  Task UpdatePermission(UserPermisionEditDto userClaimEdit);
  Task ChangeCurrentTime(ChangeCurrentTimeDto changeCurrentTimeDto);


}

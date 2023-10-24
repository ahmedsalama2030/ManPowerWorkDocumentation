using System.Security.Claims;
using Application.Common.Pagination;
using Application.Dtos.Auth.roles;
using Application.Dtos.Auth.Users;
using Application.Dtos.Message;
using Application.Dtos.roles;
using Application.Dtos.Users;
using Application.Helper.ExtentionMethod;
using Application.IBusiness.Common;
using Application.IBusiness.Management;
using Application.Services;
using AutoMapper;
using Core.Common.Dto;
using Core.Entities.Management;
using Core.Enums;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
namespace Infrastructure.Business.Management;
public class UserBusiness : IUserBusiness
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _rolemanager;
    private readonly IRepositoryApp<User> _useRepo;
    private readonly IMapper _mapper;
    private readonly IClockService _ClockService;
    private readonly IHttpContextAccessor _accssor;

    private readonly IStringLocalizerCustom _localizer;
    private readonly ILogCustom _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepositoryMessage _iRepositoryMessage;
    private readonly IConfiguration _iConfiguration;

    public UserBusiness(
       UserManager<User> User,
       RoleManager<Role> Role,
       IRepositoryApp<User> useRepo,
        IMapper mapper,
       IHttpContextAccessor httpContextAccessor,
       IStringLocalizerCustom localizer,
       ILogCustom logger,
       IRepositoryMessage iRepositoryMessage,
       IClockService clockService,
            IHttpContextAccessor Accssor,
IConfiguration iConfiguration
       )
    {
        _mapper = mapper;
        _localizer = localizer;
        _logger = logger;
        _userManager = User;
        _rolemanager = Role;
        _useRepo = useRepo;
        _httpContextAccessor = httpContextAccessor;
        _iRepositoryMessage = iRepositoryMessage;
        _ClockService = clockService;
        _accssor = Accssor;
        _iConfiguration = iConfiguration;
    }
    public async Task<IEnumerable<UserListDto>> Get(HttpResponse Response, UserParam paginationParam)
    {
         var users = _userManager.Users.Where(a => a.IsDeleted == false);
            Filter(ref users, paginationParam);
         Sort(ref users, paginationParam);
        var usersReturn = _mapper.ProjectTo<UserListDto>(users);
        var PagedList = await PagedList<UserListDto>.CreateAsync(usersReturn, paginationParam.pageNumber, paginationParam.PageSize);
        Response.AddPagination(PagedList.TotalItems, PagedList.TotalPages);
        _logger.Info<User>(MessageReturn.Common_SearchFor, paginationParam);
        return PagedList;
    }
    public async Task<IEnumerable<UserListDto>> GetTeamWork(HttpResponse Response, UserParam paginationParam)
    {
        var userTenantId = int.Parse(_accssor.HttpContext.User.FindFirstValue("TenantId"));
         var userId = int.Parse(_accssor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
         var userTenantMainId= await _useRepo.GetAll(a=>a.UserTenantId==userTenantId).AsNoTracking().Select(a=>a.Id).FirstOrDefaultAsync();

         var users = _userManager.Users.Where(a =>a.UserTenantId==userTenantId&&a.Id!=userTenantMainId);
            Filter(ref users, paginationParam);
         Sort(ref users, paginationParam);

        var usersReturn = _mapper.ProjectTo<UserListDto>(users);
        var PagedList = await PagedList<UserListDto>.CreateAsync(usersReturn, paginationParam.pageNumber, paginationParam.PageSize);
        Response.AddPagination(PagedList.TotalItems, PagedList.TotalPages);
        _logger.Info<User>(MessageReturn.Common_SearchFor, paginationParam);
        return PagedList;
    }
      public async Task<RepositoryMessage> GetByClient(Guid id)
    {
        var userQ =   _useRepo.GetAll(a => a.ClientId == id).AsNoTracking();
        var userM = _mapper.ProjectTo<UserGetDto>(userQ);
        var user = await userM.FirstOrDefaultAsync();
        if (user == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        return _iRepositoryMessage.SuccessMessage(user);
    }

    public async Task<IEnumerable<UserListDto>> GetUsersForRole(HttpResponse Response, int userId, string role, UserParam paginationParam)
    {
        //  id = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var newUser = (from user in _useRepo.GetAll(a => a.Id != userId)
                       from usersroles in user.UserRole
                       join roles in _rolemanager.Roles.Where(a => a.NameEn == role)
                       on usersroles.RoleId equals roles.Id
                       select user
                   );
        if ((!string.IsNullOrEmpty(paginationParam.filterType)) && (!string.IsNullOrEmpty(paginationParam.filterValue)))
            Filter(ref newUser, paginationParam);
        newUser.OrderBy(a => a.Name);
        //   users = Sort(users, paginationParam);
        var PagedList = await PagedList<User>.CreateAsync(newUser, paginationParam.pageNumber, paginationParam.PageSize);
        var usersReturn = _mapper.Map<IEnumerable<UserListDto>>(PagedList);
        Response.AddPagination(PagedList.TotalItems, PagedList.TotalPages);
        _logger.Info<User>("searchfor", paginationParam);
        return usersReturn;
    }
    public async Task<IEnumerable<RoleListDto>> GetRoleUser(int id)
    {
        var user = await _userManager.Users.Include(r => r.UserRole).FirstOrDefaultAsync(a => a.Id == id);
        var roleUser = (from userRole in user.UserRole
                        join role in _rolemanager.Roles
                          on userRole.RoleId equals role.Id
                        select role
       );
        var usersReturn = _mapper.Map<IEnumerable<RoleListDto>>(roleUser);
        _logger.Info<User>("searchfor", id);
        return usersReturn;
    }
    public async Task<RepositoryMessage> GetUser(int id)
    {
        var userQ = _userManager.Users.Where(a => a.Id == id);
        var userM = _mapper.ProjectTo<UserGetDto>(userQ);
        var user = await userM.FirstOrDefaultAsync();
        if (user == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        return _iRepositoryMessage.SuccessMessage(user);
    }
    public async Task AssignRoles(int userId, params string[] roles)
    {
        var rolesNew = new List<string>();
        var user = await _userManager.FindByIdAsync(userId.ToString());
        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            var roleDb = _rolemanager.Roles.FirstOrDefault(a => a.NameEn == role);
            if (roleDb == null)
                throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
            rolesNew.Add(roleDb.Name);
        }
        roles = roles ?? new string[] { };
        var result = await _userManager.AddToRolesAsync(user, rolesNew.Except(userRoles));
        result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(rolesNew));

    }
    public async Task ChangePassword(int id, PaaswordDto newpassword)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        await _userManager.RemovePasswordAsync(user);
        await _userManager.AddPasswordAsync(user, newpassword.Password);
        _logger.Info<User>(MessageReturn.Common_SuccessEdit, "",AuditType.edit,"*******","*******");
    }

    public async Task DeleteById(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        user.IsDeleted = true;
        await _userManager.UpdateAsync(user);
        _logger.Info<User>(MessageReturn.Common_SuccessDelete, "",AuditType.delete,user);
    }
    public async Task Edit(int id, UserEditDto userEdit)
    {
        var user = _userManager.Users.FirstOrDefault(a => a.Id == id);
        if (user == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
      var   userMapped = _mapper.Map(userEdit, user);
        userMapped.LastModificationTime = DateTime.Now;
        var result = await _userManager.UpdateAsync(userMapped);
        if (result.Succeeded)
        {
        _logger.Info<User>(MessageReturn.Common_SuccessEdit, "",AuditType.edit,user,userEdit);
          var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRoleAsync(user, userEdit.RoleName);
        }

    }
     public async Task TeamWorkEdit(int id, UserEditDto userEdit)
    {
        var user = _userManager.Users.FirstOrDefault(a => a.Id == id);
        if (user == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
      var   userMapped = _mapper.Map(userEdit, user);
        userMapped.LastModificationTime = DateTime.Now;
        var result = await _userManager.UpdateAsync(userMapped);
        if (result.Succeeded)
        _logger.Info<User>(MessageReturn.Common_SuccessEdit, "",AuditType.edit,user,userEdit);
           

    }
    public async Task DeleteRange(int[] users)
    {
        foreach (var id in users)
        {
            var fullUsers = await _useRepo.GetByIdAsync(id);
             if (fullUsers == null)
                throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
            fullUsers.IsDeleted = true;
            _useRepo.Update(fullUsers);
            await _useRepo.SaveAllAsync();
        _logger.Info<User>(MessageReturn.Common_SuccessDelete, "",AuditType.delete,users);

        }
    }
    public async Task DeleteHard(int[] users)
    {
         var usersDeleted=new List<User>();
        foreach (var id in users)
        {
            var fullUsers = await _useRepo.GetByIdAsync(id);
             if (fullUsers != null)
            usersDeleted.Add(fullUsers);
        _logger.Info<User>(MessageReturn.Common_SuccessDelete, "",AuditType.delete,users);

        }
        _useRepo.DeleteRange(usersDeleted);
         await _useRepo.SaveAllAsync();
 }
   public async Task<UserAnalysis> UsersAnalysis()
    {
        var userAnalysis=new UserAnalysis();
          userAnalysis.AllUsers =await  _useRepo.GetAll().IgnoreQueryFilters().CountAsync();
          userAnalysis.ActiveUsers =await  _useRepo.GetAll(a=>a.LockoutEnd<_ClockService.Now).CountAsync();
          userAnalysis.LockedUsers =await  _useRepo.GetAll(a=>a.LockoutEnd>=_ClockService.Now).CountAsync();
          userAnalysis.DeletedUsers =await  _useRepo.GetAll().IgnoreQueryFilters().CountAsync(a=>a.IsDeleted);
        return userAnalysis;
    }

    private void Filter(ref IQueryable<User> User, UserParam paginationParam)
    {
     if ((!string.IsNullOrEmpty(paginationParam.filterType)) && (!string.IsNullOrEmpty(paginationParam.filterValue)))
      User = User.Where(a => a.Name.Contains(paginationParam.filterValue) || a.UserName.Contains(paginationParam.filterValue) || a.Email.Contains(paginationParam.filterValue)   );

      if(paginationParam.IsAllUsers)
      User=User.IgnoreQueryFilters();
      if(paginationParam.IsActiveUsers)
      User=User.Where(a=>a.LockoutEnd<_ClockService.Now);
      if(paginationParam.IsLocked)
      User=User.Where(a=>a.LockoutEnd>=_ClockService.Now);
       if(paginationParam.IsDeletedUsers)
      User=User.IgnoreQueryFilters().Where(a=>a.IsDeleted);
 
    }
    private void Sort(ref IQueryable<User> Users, UserParam paginationParam)
    {
       if ((!string.IsNullOrEmpty(paginationParam.filterType)) && (!string.IsNullOrEmpty(paginationParam.sortType))){
        switch (paginationParam.filterType)
        {
            case "fullName": Users = paginationParam.sortType == "asc" ? Users.OrderBy(a => a.Name) : Users.OrderByDescending(a => a.Name); break;
            case "userName": Users = paginationParam.sortType == "asc" ? Users.OrderBy(a => a.UserName) : Users.OrderByDescending(a => a.UserName); break;
            case "email": Users = paginationParam.sortType == "asc" ? Users.OrderBy(a => a.Email) : Users.OrderByDescending(a => a.Email); break;
            default: Users=Users.OrderBy(a=>a.Id); break;
        }
       }
    }

    public async Task<List<BaseListDto>> GetAllList()
    {
        var userTenantId = int.Parse(_accssor.HttpContext.User.FindFirstValue("TenantId"));
          var result= await _useRepo.GetAllAsync(a=>a.UserTenantId==userTenantId);
          return _mapper.Map<List<BaseListDto>>(result);
    }
    public async Task UpdatePermission(UserPermisionEditDto userClaimEdit)
    {
        var user=await _useRepo.SingleOrDefaultAsync(a=>a.ClientId==userClaimEdit.UserId);
        if (user == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        var userClaims = (await _userManager.GetClaimsAsync(user)).DistinctBy(a=>a.Value);
       await _userManager.RemoveClaimsAsync(user, userClaims);
        foreach (var permision in userClaimEdit.Permisions)
            await _userManager.AddClaimAsync(user, new Claim(permision.Type, permision.Value));
        _logger.Info<User>(MessageReturn.Common_SuccessEdit, "",AuditType.register,userClaimEdit);
  
    }

   
   
    public async Task ChangeCurrentTime(ChangeCurrentTimeDto changeCurrentTimeDto)
    {
        var id = _accssor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(id.ToString()) ?? throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        user.LastLogin= changeCurrentTimeDto.Date;
          _useRepo.Update(user);
         await _useRepo.SaveAllAsync(false);
    }
}

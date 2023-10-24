using System.Security.Claims;
using Application.Common.Pagination;
using Application.Dtos.Auth.roles;
using Application.Dtos.Message;
using Application.Dtos.roles;
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
using Newtonsoft.Json;
namespace Infrastructure.Business.Management;
public class RoleBusiness : IRoleBusiness
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IRepositoryApp<Role> _roleRepo;
    private readonly IRepositoryMessage _iRepositoryMessage;
    private readonly ILogCustom _logger;
    private readonly IStringLocalizerCustom _localizer;
    private readonly IHttpContextAccessor _accssor;
    public RoleBusiness(
       RoleManager<Role> Role,
       IMapper mapper,
       IRepositoryApp<Role> RoleRepo,
       IRepositoryMessage IRepositoryMessage,
       ILogCustom logger,
       IStringLocalizerCustom localizer,
       IHttpContextAccessor accssor,
       UserManager<User> userManager = null)
    {
        _mapper = mapper;
        _roleRepo = RoleRepo;
        _iRepositoryMessage = IRepositoryMessage;
        _logger = logger;
        _localizer = localizer;
        _roleManager = Role;
        _accssor = accssor;
        _userManager = userManager;

    }

    public async Task Delete(int id)
    {
        var role = _roleManager.Roles.FirstOrDefault(a => a.Id == id);
        if (role == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        await _roleManager.DeleteAsync(role);
    }
   public async Task DeleteRange(params int[] roles)
    {
        foreach (var id in roles)
        {
            var role = await _roleRepo.GetByIdAsync(id);
            if (role == null)
                throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
            role.IsDeleted = true;
            
            _roleRepo.Update(role);
            await _roleRepo.SaveAllAsync();
        }
    }

    public async Task Edit(int id, RoleRegisterDto RoleRegister)
    {
        var role = await _roleRepo.GetByIdAsync(id);
        if (role == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
       var roleMapped = _mapper.Map(RoleRegister, role);
        role.LastModificationTime = DateTime.Now;
        await _roleManager.UpdateAsync(role);
        _logger.Info<User>(MessageReturn.Common_SuccessEdit, "",AuditType.edit,roleMapped);


    }

    public async Task<IEnumerable<RoleListDto>> Get(HttpResponse Response, PaginationParam paginationParam)
    {
        var roles = _roleManager.Roles.Where(d => d.IsDeleted == false);
          Filter(ref roles, paginationParam);
        Sort(ref roles, paginationParam);
        var PagedList = await PagedList<Role>.CreateAsync(roles, paginationParam.pageNumber, paginationParam.PageSize);
        var UsersReturn = _mapper.Map<IEnumerable<RoleListDto>>(PagedList);
        Response.AddPagination(PagedList.TotalItems, PagedList.TotalPages);
        _logger.Info<Role>(MessageReturn.Common_SearchFor,paginationParam);
        return UsersReturn;
    }
    public virtual async Task<List<BaseListDto>> GetAllList()
    {
        var repo = _roleRepo.GetAll();
        var entities = await _mapper.ProjectTo<BaseListDto>(repo).ToListAsync();
        return entities;
    }
    public async Task<IEnumerable<RoleListDto>> GetAll()
    {
        var roles = await _roleManager.Roles.Where(a => a.IsDeleted == false).ToListAsync();
        var rolesReturn = _mapper.Map<IEnumerable<RoleListDto>>(roles);
        return rolesReturn;
    }
    public async Task<RepositoryMessage> GetRole(int id)
    {
        var role = await _roleManager.Roles.FirstOrDefaultAsync(a => a.IsDeleted == false && a.Id == id);
        if (role == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        var rolesReturn = _mapper.Map<RoleListEditDto>(role);
        return _iRepositoryMessage.SuccessMessage(rolesReturn);
    }
    public async Task  Register(RoleRegisterDto RoleRegister)
    {
        var roleStatus=await _roleRepo.SingleOrDefaultAsNoTrackingAsync(x => x.Name == RoleRegister.Name || x.NameEn == RoleRegister.NameEn || x.NameAr == RoleRegister.NameAr);
         if (roleStatus!=null)
            throw new ExceptionCommonReponse(MessageReturn.Mangement_RoleFound, 400);
        var role = _mapper.Map<Role>(RoleRegister);
        role.LastModificationTime = role.CreationTime = DateTime.Now;
        await _roleManager.CreateAsync(role);
         var returnRole = _mapper.Map<RoleListDto>(role);
        _logger.Info<User>(MessageReturn.Common_SuccessRegister, "",AuditType.register,RoleRegister);

      }
    public async Task UpdatePermission(RolePermisionEditDto roleClaimEdit)
    {
        var role = await _roleRepo.GetByIdAsync(roleClaimEdit.RoleId);
        if (role == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        var roleClaims = await _roleManager.GetClaimsAsync(role);
        foreach (var claim in roleClaims)
            await _roleManager.RemoveClaimAsync(role, claim);
            
        foreach (var permision in roleClaimEdit.Permisions)
            await _roleManager.AddClaimAsync(role, new Claim(permision.Type, permision.Value));
        _logger.Info<User>(MessageReturn.Common_SuccessEdit, "",AuditType.register,roleClaimEdit);

  
    }

    private void Filter(ref IQueryable<Role> Roles, PaginationParam paginationParam)
    {
     if ((!string.IsNullOrEmpty(paginationParam.filterType)) && (!string.IsNullOrEmpty(paginationParam.filterValue)))
         Roles = Roles.Where(a => a.Name.Contains(paginationParam.filterValue) || a.NameAr.Contains(paginationParam.filterValue));
    }
    private void Sort(ref IQueryable<Role> Roles, PaginationParam paginationParam)
    {
       if ((!string.IsNullOrEmpty(paginationParam.filterType)) && (!string.IsNullOrEmpty(paginationParam.sortType))){
        switch (paginationParam.filterType)
        {
            case "name": Roles = paginationParam.sortType == "asc" ? Roles.OrderBy(a => a.Name) : Roles.OrderByDescending(a => a.Name); break;
            case "nameAr": Roles = paginationParam.sortType == "asc" ? Roles.OrderBy(a => a.NameAr) : Roles.OrderByDescending(a => a.NameAr); break;
            default: Roles=Roles.OrderBy(a=>a.Id); break;
         }
       }
    }

    
}

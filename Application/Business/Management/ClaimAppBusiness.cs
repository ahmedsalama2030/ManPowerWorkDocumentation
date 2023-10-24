
using Application.Common.Pagination;
using Application.Dtos.Auth.ClaimApp;
using Application.Dtos.Message;
using Application.IBusiness.Common;
using Application.IBusiness.Management;
using Application.Business.Common;
using Application.Services;
using AutoMapper;
using Core.Entities.Management;
using Core.Interfaces;
using Core.Interfaces.Common;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Application.IRpository.Managment;
using Microsoft.AspNetCore.Identity;
using Core.Exceptions;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.Management;
public class ClaimAppBusiness : EntitiesBusinessCommon<
ClaimApp,
ClaimAppGetDto,
ClaimAppEditDto,
ClaimAppEditDto,
PaginationParam
>, IClaimAppBusiness
{
    private readonly IRepoClaimApp _iRepoClaimApp;
    private readonly IRepositoryApp<ModuleApp> _repoModuleApp;
    private readonly RoleManager<Role> _roleManager;
    private readonly IRepositoryApp<RoleClaim> _RoleClaim;
    private readonly IRepositoryApp<User> _userRepo;
    private readonly IRepositoryApp<UserRole> _userRole;
    private readonly IRepositoryApp<Role> _roleRepo;
    private readonly IRepositoryApp<ClaimAppView> _ClaimAppView;
    private readonly UserManager<User> _userManager;

    public ClaimAppBusiness(
          IRepoClaimApp IRepoClaimApp,
         IRepositoryApp<ModuleApp> RepoModuleApp,
         IMapper mapper,
         IHttpContextAccessor accssor,
         ILogCustom LogCustom,
         IStringLocalizerCustom localizer,
         IRepositoryMessage iRepositoryMessage,
          IClockService iClockService,
            RoleManager<Role> RoleManager,
        IRepositoryApp<Role> RoleRepo
,
        IRepositoryApp<ClaimAppView> ClaimAppView,
        UserManager<User> userManager,
        IRepositoryApp<User> userRepo,
        IRepositoryApp<UserRole> userRole,
        IRepositoryApp<RoleClaim> roleClaim) : base(IRepoClaimApp, mapper, accssor, LogCustom, localizer, iRepositoryMessage, iClockService)
    {
        _iRepoClaimApp = IRepoClaimApp;
        _repoModuleApp = RepoModuleApp;
        _roleRepo = RoleRepo;
        _roleManager = RoleManager;
        _ClaimAppView = ClaimAppView;
        _userManager = userManager;
        _userRepo = userRepo;
        _userRole = userRole;
        _RoleClaim = roleClaim;
    }
    public override async Task Register(ClaimAppEditDto TRegister)
    {
        var entityFound = await _repo.SingleOrDefaultAsNoTrackingAsync(a => a.ClaimValue == TRegister.ClaimValue || a.Key == TRegister.Key && a.ScreenAppId == TRegister.ScreenAppId);
        if (entityFound != null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        var entity = _mapper.Map<ClaimApp>(TRegister);
        entity.ClaimType = entity.ClaimType.ToLower();
        entity.ClaimValue = entity.ClaimValue.ToLower();
        LogRowRegister(ref entity);
        _repo.Add(entity);
        await _repo.SaveAllAsync();
    }

    public override async Task Edit(int id, ClaimAppEditDto entityEdit)
    {
        var entityFound = await _repo.SingleOrDefaultAsNoTrackingAsync(a => a.ClaimValue == entityEdit.ClaimValue && a.Id != id || a.Key == entityEdit.Key && a.ScreenAppId == entityEdit.ScreenAppId && a.Id != id);
        if (entityFound != null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        var entityOld = _mapper.Map<ClaimApp>(entity);
        _mapper.Map(entityEdit, entity);
        LogRowEdit(ref entity);
        entity.ClaimType = entity.ClaimType.ToLower();
        entity.ClaimValue = entity.ClaimValue.ToLower();
        _repo.Update(entity);
        await _repo.SaveAllAsync();
    }
    public async Task<HashSet<ClaimsSelect>> GetClaimsOrg(int roleId)
    {
        var role = await _roleRepo.GetByIdAsync(roleId);
        var roleClaims = await _roleManager.GetClaimsAsync(role);
        var cliams = await _ClaimAppView.GetAllAsync();
        var result = cliams.GroupBy(a => a.ModuleAppId);
        var claimsSelects = new HashSet<ClaimsSelect>();
        foreach (var module in result)
        {
            var claimSelect = new ClaimsSelect();
            claimSelect = _mapper.Map<ClaimsSelect>(module.ToList().FirstOrDefault());
            var screens = module.ToList().GroupBy(a => a.ScreenAppId);
            foreach (var screen in screens)
            {
                var screenMapped = _mapper.Map<ScreenClaim>(screen.ToList().FirstOrDefault());
                var cliamsMapped = _mapper.Map<HashSet<ClaimAppEditDto>>(screen.ToList().OrderBy(a => a.Key));
                foreach (var claim in cliamsMapped)
                {
                    var claimStatus = roleClaims.Any(a => a.Value.ToLower() == claim.ClaimValue.ToLower());
                    if (claimStatus)
                        claim.IsSelected = true;
                    screenMapped.claims.Add(claim);
                }
                claimSelect.ScreenClaims.Add(screenMapped);
            }
            claimsSelects.Add(claimSelect);
        }
        return claimsSelects;
    }
    public async Task<HashSet<ClaimsSelect>> GetUserClaimsOrg(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        var userClaims = await _userManager.GetClaimsAsync(user);
        var cliams = await _ClaimAppView.GetAllAsync();
        var result = cliams.GroupBy(a => a.ModuleAppId);
        var claimsSelects = new HashSet<ClaimsSelect>();
        foreach (var module in result)
        {
            var claimSelect = new ClaimsSelect();
            claimSelect = _mapper.Map<ClaimsSelect>(module.ToList().FirstOrDefault());
            var screens = module.ToList().GroupBy(a => a.ScreenAppId);
            foreach (var screen in screens)
            {
                var screenMapped = _mapper.Map<ScreenClaim>(screen.ToList().FirstOrDefault());
                var cliamsMapped = _mapper.Map<HashSet<ClaimAppEditDto>>(screen.ToList().OrderBy(a => a.Key));
                foreach (var claim in cliamsMapped)
                {
                    var claimStatus = userClaims.Any(a => a.Value.ToLower() == claim.ClaimValue.ToLower());
                    if (claimStatus)
                        claim.IsSelected = true;
                    screenMapped.claims.Add(claim);
                }
                claimSelect.ScreenClaims.Add(screenMapped);
            }
            claimsSelects.Add(claimSelect);
        }
        return claimsSelects;
    }



    public async Task<HashSet<ClaimsSelect>> GetUserClaimsForRoleOrg(Guid userId)
    {
        var userTenantId = int.Parse(_accssor.HttpContext.User.FindFirstValue("TenantId"));
        var user = await  _userRepo.SingleOrDefaultAsNoTrackingAsync(a=>a.ClientId==userId);
         var claimsTenant=await GetUserClaimsTenant(userTenantId);
        var cliams = await _ClaimAppView.GetAllAsync(a => claimsTenant.Contains(a.ClaimValue));
        var result = cliams.GroupBy(a => a.ModuleAppId);
        var userClaims = await _userManager.GetClaimsAsync(user);
        var claimsSelects = new HashSet<ClaimsSelect>();
        foreach (var module in result)
        {
            var claimSelect = new ClaimsSelect();
            claimSelect = _mapper.Map<ClaimsSelect>(module.ToList().FirstOrDefault());
            var screens = module.ToList().GroupBy(a => a.ScreenAppId);
            foreach (var screen in screens)
            {
                var screenMapped = _mapper.Map<ScreenClaim>(screen.ToList().FirstOrDefault());
                var cliamsMapped = _mapper.Map<HashSet<ClaimAppEditDto>>(screen.ToList().OrderBy(a => a.Key));
                foreach (var claim in cliamsMapped)
                {
                    var claimStatus = userClaims.Any(a => a.Value.ToLower() == claim.ClaimValue.ToLower());
                    if (claimStatus)
                        claim.IsSelected = true;
                    screenMapped.claims.Add(claim);
                }
                claimSelect.ScreenClaims.Add(screenMapped);
            }
            claimsSelects.Add(claimSelect);
        }
        return claimsSelects;
    }
    private  async Task< List<string>>   GetUserClaimsTenant(int userTenantId){
 var claimsTenant=await ( from userRoleSelect in  _userRole.GetAll() 
                       join userSelect in  _userRepo.GetAll(a=>a.UserTenantId==userTenantId)  on userRoleSelect.UserId equals userSelect.Id
                       join role in _roleRepo.GetAll().AsNoTracking()on userRoleSelect.RoleId equals role.Id
                       join roleClaim  in _RoleClaim.GetAll().AsNoTracking() on role.Id equals roleClaim.RoleId
                    select roleClaim).Select(a=>a.ClaimValue).ToListAsync();
                    return claimsTenant;
    }


    public override void Filter(ref IQueryable<ClaimApp> entities, PaginationParam paginationParam)
    {
        if (!string.IsNullOrEmpty(paginationParam.filterValue))
            entities = entities.Where(a =>
                  a.ClaimType.Contains(paginationParam.filterValue)
               || a.ClaimValue.Contains(paginationParam.filterValue)
               || a.Key.Contains(paginationParam.filterValue)
               || a.ScreenApp.NameAr.Contains(paginationParam.filterValue)
               || a.ScreenApp.NameEn.Contains(paginationParam.filterValue)
               );
    }
    public override void Sort(ref IQueryable<ClaimApp> entities, PaginationParam paginationParam)
    {
        if ((!string.IsNullOrEmpty(paginationParam.filterType)) && (!string.IsNullOrEmpty(paginationParam.sortType)))
        {
            switch (paginationParam.filterType)
            {
                case "claimType": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.ClaimType) : entities.OrderByDescending(a => a.ClaimType); break;
                case "claimValue": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.ClaimValue) : entities.OrderByDescending(a => a.ClaimValue); break;
                case "key": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.Key) : entities.OrderByDescending(a => a.Key); break;
                case "screenAppNameEn": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.ScreenApp.NameEn) : entities.OrderByDescending(a => a.ScreenApp.NameEn); break;
                case "screenAppNameAr": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.ScreenApp.NameAr) : entities.OrderByDescending(a => a.ScreenApp.NameAr); break;
                case "creationTime": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.CreationTime) : entities.OrderByDescending(a => a.CreationTime); break;
                case "lastModificationTime": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.LastModificationTime) : entities.OrderByDescending(a => a.LastModificationTime); break;
                default: entities = entities.OrderBy(a => a.Id); break;
            }
        }
    }
}

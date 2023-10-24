using System;
using System.Linq.Expressions;
using Application.Common.Pagination;
using Application.Dtos.Auth.Audit;
using Application.Helper.ExtentionMethod;
using Application.IBusiness.Common;
using Application.IBusiness.Management;
using Application.Services;
using AutoMapper;
using Core.Entities.Management;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Common;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.Management;
public class AuditAppBusiness : IAuditAppBusiness
{
    protected readonly IRepositoryAppHepler<Audit> _repo;
    protected readonly IMapper _mapper;
    protected readonly IHttpContextAccessor _accssor;
    protected readonly ILogCustom _logger;
    protected readonly IStringLocalizerCustom _localizer;
    protected readonly IRepositoryMessage _iRepositoryMessage;
    protected readonly IClockService _iClockService;
    public AuditAppBusiness(
   IRepositoryAppHepler<Audit> Repo,
   IMapper mapper,
   IHttpContextAccessor accssor,
   ILogCustom logger,
   IStringLocalizerCustom localizer,
  IRepositoryMessage IRepositoryMessage,
  IClockService iClockService)
    {
        _repo = Repo;
        _mapper = mapper;
        _accssor = accssor;
        _logger = logger;
        _localizer = localizer;
        _iRepositoryMessage = IRepositoryMessage;
        _iClockService = iClockService;
    }
    public async Task<List<AuditGetDto>> Get(HttpResponse Response, AuditParam paginationParam)
    {
        var entities = _repo.GetAll();
        // var s=await _repo.GetAllAsync();
        Filter(ref entities, paginationParam);
        Sort(ref entities, paginationParam);
        var entitiesMapped = _mapper.ProjectTo<AuditGetDto>(entities);

        var PagedList = await PagedList<AuditGetDto>.CreateAsync(entitiesMapped, paginationParam.pageNumber, paginationParam.PageSize);
        Response.AddPagination(PagedList.TotalItems, PagedList.TotalPages);
        object paramFilter = string.IsNullOrEmpty(paginationParam.filterValue) ? null : paginationParam;
        return PagedList.ToList();
    }
    public async Task<Audit> GetByIdAsync(int id)
    {
        var entity = await _repo.SingleOrDefaultAsNoTrackingAsync(a => a.Id == id);
        if (entity == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        return entity;
    }
    public async Task<List<AuditList>> GetTableNameAsync()
    {
        return await _repo.GetAll().Select(a => new AuditList { Id = a.TableName, name = a.TableName }).Distinct().ToListAsync();
    }
    public async Task<List<AuditList>> GetStateAsync()
    {
        return await _repo.GetAll().Select(a => new AuditList { Id = a.State, name = a.State }).Distinct().ToListAsync();
    }
    public async Task<List<AuditList>> GetLevelAsync()
    {
        return await _repo.GetAll().Select(a => new AuditList { Id = a.Level, name = a.Level }).Distinct().ToListAsync();
    }
    public virtual void Filter(ref IQueryable<Audit> entities, AuditParam paginationParam)
    {
         if (paginationParam.DateFrom != null && paginationParam.DateTo != null )
            entities = entities.Where(a =>a.TimeStamp>=paginationParam.DateFrom&&a.TimeStamp<=paginationParam.DateFrom);
         if (!string.IsNullOrEmpty(paginationParam.UserFullName))
            entities = entities.Where(a => a.UserFullName.Contains(paginationParam.UserFullName));
        if (!string.IsNullOrEmpty(paginationParam.RowClientId))
            entities = entities.Where(a => a.RowClientId == paginationParam.RowClientId);
        if (paginationParam.StatusCode != null)
            entities = entities.Where(a => a.StatusCode == (int)paginationParam.StatusCode);
        if (paginationParam.TablesName != null && paginationParam.TablesName.Any())
            entities = entities.Where(a => paginationParam.TablesName.Contains(a.TableName));
        if (paginationParam.Levels != null && paginationParam.Levels.Any())
            entities = entities.Where(a => paginationParam.Levels.Contains(a.Level));
        if (paginationParam.States != null && paginationParam.States.Any())
            entities = entities.Where(a => paginationParam.States.Contains(a.State));
        if (paginationParam.IsLogin == true)
            entities = entities.Where(a => a.IsLogin == true);
        if (paginationParam.IsLogout == true)
            entities = entities.Where(a => a.IsLogout == true);
        if (paginationParam.IsShowUser == true)
            entities = entities.Where(a => a.IsShowUser == true);
    }
   
   
   
    public virtual void Sort(ref IQueryable<Audit> entities, IPaginationParam paginationParam)
    {
        if ((!string.IsNullOrEmpty(paginationParam.filterType)) && (!string.IsNullOrEmpty(paginationParam.sortType)))
        {
            var parameter = Expression.Parameter(typeof(Audit), "x");
            var property = Expression.PropertyOrField(parameter, paginationParam.filterType);
            var lambda = Expression.Lambda(property, parameter);
            string methodOrder = paginationParam.sortType == "asc" ? "OrderBy" : "OrderByDescending";
            var method = typeof(Queryable).GetMethods()
               .Where(m => m.Name == methodOrder)
               .Single(m => m.GetParameters().Length == 2)
               .MakeGenericMethod(typeof(Audit), property.Type);
            entities = (IQueryable<Audit>)method.Invoke(null, new object[] { entities, lambda });
        }
    }
}


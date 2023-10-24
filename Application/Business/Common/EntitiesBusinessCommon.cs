using Application.Common.Pagination;
using Application.Helper.ExtentionMethod;
using AutoMapper;
using Core.Common;
using Core.Common.Dto;
using Core.Interfaces;
using Core.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using Application.Dtos.Message;
using Application.IBusiness.Common;
using FluentValidation.Results;
using Application.Services;
using System.Linq.Expressions;
using System.Reflection;
using Core.Exceptions;

namespace Application.Business.Common;
public class EntitiesBusinessCommon<T, TDtoGet, TRegister, TEdit, TPagination> :
IEntitiesBusinessCommon<T, TDtoGet, TRegister, TEdit, TPagination>
where T : BaseEntity
where TDtoGet : class
where TRegister : class
where TEdit : class
where TPagination : IPaginationParam
{
    #region  init
    protected readonly IRepositoryApp<T> _repo;
    protected readonly IMapper _mapper;
    protected readonly IHttpContextAccessor _accssor;
    protected readonly ILogCustom _logger;
    protected readonly IStringLocalizerCustom _localizer;
    protected readonly IRepositoryMessage _iRepositoryMessage;
    protected readonly IClockService _iClockService;
    public EntitiesBusinessCommon(
    IRepositoryApp<T> Repo,
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
    #endregion
    public virtual async Task<List<TDtoGet>> Get(HttpResponse Response, TPagination paginationParam)
    {
        var entities = _repo.GetAll();
        Filter(ref entities, paginationParam);
        Sort(ref entities, paginationParam);
        var entitiesMapped = _mapper.ProjectTo<TDtoGet>(entities);
        var PagedList = await PagedList<TDtoGet>.CreateAsync(entitiesMapped, paginationParam.pageNumber, paginationParam.PageSize);
        Response.AddPagination(PagedList.TotalItems, PagedList.TotalPages);
        object paramFilter=string.IsNullOrEmpty(paginationParam.filterValue)?null:paginationParam;
        _logger.Info<T>(MessageReturn.Common_SearchFor, paramFilter);
        return PagedList.ToList();
    }
    public virtual async Task<List<TDtoGet>> GetAll(TPagination paginationParam)
    {
        var entities = _repo.GetAll();
        if ((!string.IsNullOrEmpty(paginationParam.filterType)) && (!string.IsNullOrEmpty(paginationParam.filterValue)))
            Filter(ref entities, paginationParam);
        entities.OrderBy(a => a.CreationTime);
        var entitiesMapped = _mapper.ProjectTo<TDtoGet>(entities);
        var newEntitiesDto = await entitiesMapped.ToListAsync();
        return newEntitiesDto;
    }
    public virtual async Task<List<BaseListDto>> GetAllList()
    {
        var repo = _repo.GetAll().AsNoTracking();
        var entities = await _mapper.ProjectTo<BaseListDto>(repo).ToListAsync();
        return entities;
    }
    public virtual async Task<TDtoGet> GetByIdAsync(int id)
    {
        var entity = _repo.GetAll(a => a.Id == id);
        var entitiesMapped = _mapper.ProjectTo<TDtoGet>(entity);
        var result = await entitiesMapped.FirstOrDefaultAsync();
        if (result == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        return result;
    }
    public virtual async Task<double> GetCount()
    {
        return await _repo.Count();
    }
    public virtual async Task Register(TRegister TRegister)
    {
        var entity = _mapper.Map<T>(TRegister);
        LogRowRegister(ref entity);
        _repo.Add(entity);
        await _repo.SaveAllAsync();
    }
      public virtual void RegisterNoAsync(TRegister TRegister)
    {
        var entity = _mapper.Map<T>(TRegister);
        LogRowRegister(ref entity);
        _repo.Add(entity);
          _repo.SaveAll();
    }
    public virtual async Task Edit(int id, TEdit entityEdit)
    {
        var entity = await _repo.GetByIdAsync(id) ?? throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        var entityOld = _mapper.Map(entityEdit, entity);
        LogRowEdit(ref entityOld);
        _repo.Update(entityOld);
        await _repo.SaveAllAsync();
    }
    public virtual async Task DeleteById(int id)
    {
        var entity = await _repo.GetByIdAsync(id) ?? throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        _repo.Delete(entity);
        await _repo.SaveAllAsync();
    }
    public virtual async Task DeleteRangeSoft(int[] arrayObjects)
    {
        List<T> entities = new List<T>();
        foreach (var id in arrayObjects)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                LogRowEdit(ref entity);
                entities.Add(entity);
            }
        }
        if (entities.Any())
        {
            _repo.UpdateRange(entities);
            await _repo.SaveAllAsync();
        }
    }
    public virtual async Task DeleteRange(int[] arrayObjects)
    {
        List<T> entities = new();
        foreach (var id in arrayObjects)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity != null)
                entities.Add(entity);
        }
        if (entities.Any())
        {
            _repo.DeleteRange(entities);
            await _repo.SaveAllAsync();
        }
    }

    public virtual async Task DeleteSoftById(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        entity.IsDeleted = true;
        LogRowEdit(ref entity);

        _repo.Update(entity);
        await _repo.SaveAllAsync();
    }
    public virtual void Filter(ref IQueryable<T> entities, TPagination paginationParam) { }
    public virtual void Sort(ref IQueryable<T> entities, TPagination paginationParam)
    {
        if ((!string.IsNullOrEmpty(paginationParam.filterType)) && (!string.IsNullOrEmpty(paginationParam.sortType)))
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, paginationParam.filterType);
            var lambda = Expression.Lambda(property, parameter);
            string methodOrder = paginationParam.sortType == "asc" ? "OrderBy" : "OrderByDescending";
            var method = typeof(Queryable).GetMethods()
               .Where(m => m.Name == methodOrder)
               .Single(m => m.GetParameters().Length == 2)
               .MakeGenericMethod(typeof(T), property.Type);
          entities = (IQueryable<T>)method.Invoke(null, new object[] { entities, lambda });
        }
    }
    protected void LogRowRegister(ref T entity)
    {
        var userId = int.Parse(_accssor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        entity.IPDeviceCreaded = entity.IPDeviceLastEdit = _accssor.HttpContext.Connection.RemoteIpAddress.ToString();
        entity.CreationTime = entity.LastModificationTime = _iClockService.Now;
        entity.CreatorId = entity.LastModifierId = userId;
        entity.ClientId = entity.ClientId ?? Guid.NewGuid();
    }
    protected void LogRowEdit(ref T entity)
    {
        var userId = int.Parse(_accssor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
         entity.IPDeviceLastEdit = _accssor.HttpContext.Connection.RemoteIpAddress.ToString();
        entity.LastModifierId = userId;
        entity.LastModificationTime = _iClockService.Now;
    }

    protected RepositoryMessage ErrorMessageValidation(ValidationResult validationResult)
    {
        var message = string.Join(Environment.NewLine, validationResult.Errors.ToList());
        return _iRepositoryMessage.ErrorMessageValidation(message);
    }
}

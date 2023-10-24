using Application.Common.Pagination;
using Application.Dtos.Auth.ScreenApp;
using Application.Dtos.Message;
using Application.Helper.ExtentionMethod;
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
using Core.Exceptions;
namespace Application.Business.Management;
public class ScreenAppBusiness : EntitiesBusinessCommon<
ScreenApp,
ScreenAppGetDto,
ScreenAppRegisterDto,
ScreenAppEditDto,
PaginationParam
>, IScreenAppBusiness
{
    private readonly IValidator<ScreenAppRegisterDto> _ScreenAppValidator;
    public ScreenAppBusiness(
         IRepositoryApp<ScreenApp> Repo,
         IMapper mapper,
         IHttpContextAccessor accssor,
         ILogCustom LogCustom,
         IStringLocalizerCustom localizer,
         IRepositoryMessage iRepositoryMessage,
          IClockService iClockService,
         IValidator<ScreenAppRegisterDto> ScreenAppValidator) : base(Repo, mapper, accssor, LogCustom, localizer, iRepositoryMessage, iClockService)
    {
        _ScreenAppValidator = ScreenAppValidator;
    }

    public override async Task Register(ScreenAppRegisterDto TRegister)
    {
        var validationResult = await _ScreenAppValidator.ValidateAsync(TRegister);
        if (!validationResult.IsValid)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        var entity = _mapper.Map<ScreenApp>(TRegister);
        LogRowRegister(ref entity);
        _repo.Add(entity);
        await _repo.SaveAllAsync();

    }
    public override void Filter(ref IQueryable<ScreenApp> entities, PaginationParam paginationParam)
    {
        if (!string.IsNullOrEmpty(paginationParam.filterValue))

            entities = entities.Where(
           a => a.NameAr.Contains(paginationParam.filterValue)
              || a.NameEn.Contains(paginationParam.filterValue)
              || a.ModuleApp.NameAr.Contains(paginationParam.filterValue)
              || a.ModuleApp.NameEn.Contains(paginationParam.filterValue)
               || a.Description.Contains(paginationParam.filterValue)
              );
    }

    public override void Sort(ref IQueryable<ScreenApp> entities, PaginationParam paginationParam)
    {
        if ((!string.IsNullOrEmpty(paginationParam.filterType)) && (!string.IsNullOrEmpty(paginationParam.sortType)))
        {

            switch (paginationParam.filterType)
            {
                case "nameAr": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.NameAr) : entities.OrderByDescending(a => a.NameAr); break;
                case "nameEn": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.NameEn) : entities.OrderByDescending(a => a.NameEn); break;
                case "moduleAppNameEn": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.ModuleApp.NameEn) : entities.OrderByDescending(a => a.ModuleApp.NameEn); break;
                case "moduleAppNameAr": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.ModuleApp.NameAr) : entities.OrderByDescending(a => a.ModuleApp.NameAr); break;
                case "isMain": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.IsMain) : entities.OrderByDescending(a => a.IsMain); break;
                case "isShowPermission": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.IsShowPermission) : entities.OrderByDescending(a => a.IsShowPermission); break;
                 case "creationTime": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.CreationTime) : entities.OrderByDescending(a => a.CreationTime); break;
                case "lastModificationTime": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.LastModificationTime) : entities.OrderByDescending(a => a.LastModificationTime); break;
                default: entities = entities.OrderBy(a => a.Id); break;
            }
        }
    }
}

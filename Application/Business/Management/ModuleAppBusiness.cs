using Application.Common.Pagination;
using Application.Dtos.Auth.ModuleApp;
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
using Core.Exceptions;
namespace Application.Business.Management;
public class ModuleAppBusiness : EntitiesBusinessCommon<
ModuleApp,
ModuleAppGetDto,
ModuleAppRegisterDto,
ModuleAppEditDto,
PaginationParam
>, IModuleAppBusiness
{
    private readonly IValidator<ModuleAppRegisterDto> _ModuleAppValidator;
    public ModuleAppBusiness(
         IRepositoryApp<ModuleApp> Repo,
         IMapper mapper,
         IHttpContextAccessor accssor,
         ILogCustom LogCustom,
         IStringLocalizerCustom localizer,
         IRepositoryMessage iRepositoryMessage,
          IClockService iClockService,
         IValidator<ModuleAppRegisterDto> ModuleAppValidator) : base(Repo, mapper, accssor, LogCustom, localizer, iRepositoryMessage, iClockService)
    {
        _ModuleAppValidator = ModuleAppValidator;
    }
    public override async Task Register(ModuleAppRegisterDto TRegister)
    {
        var entityFound = await _repo.SingleOrDefaultAsNoTrackingAsync(a => a.NameAr == TRegister.NameAr || a.NameEn == TRegister.NameEn || a.Key == TRegister.Key);
        if (entityFound != null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        var entity = _mapper.Map<ModuleApp>(TRegister);
        LogRowRegister(ref entity);
        _repo.Add(entity);
        await _repo.SaveAllAsync();
    }
    public override void Filter(ref IQueryable<ModuleApp> entities, PaginationParam paginationParam)
    {
        if (!string.IsNullOrEmpty(paginationParam.filterValue))
            entities = entities.Where(a =>
                  a.NameAr.Contains(paginationParam.filterValue)
               || a.NameEn.Contains(paginationParam.filterValue)
               || a.Key.Contains(paginationParam.filterValue)
               );
    }




}

using Application.Business.Common;
using Application.Common.Pagination;
using Application.Dtos.Localization.LanguageKey;
using Application.IBusiness.Common;
using Application.IBusiness.Localization;
using AutoMapper;
using Core.Entities.Management;
using Core.Interfaces;
using Core.Interfaces.Common;
using Microsoft.AspNetCore.Http;
namespace Application.Business.Localization;
public class LanguageKeyBusiness
: EntitiesBusinessCommon<
LanguageKey,
LanguageKeyGetDto,
LanguageKeyRegisterDto,
LanguageKeyEditDto,
PaginationParam
>, ILanguageKeyBusiness
{
    public LanguageKeyBusiness(
         IRepositoryApp<LanguageKey> Repo,
         IMapper mapper,
         IHttpContextAccessor accssor,
         ILogCustom LogCustom,
         IStringLocalizerCustom localizer,
         IRepositoryMessage iRepositoryMessage,
          IClockService iClockService


          ) : base(Repo, mapper, accssor, LogCustom, localizer, iRepositoryMessage, iClockService)
    { }
    public override async Task Register(LanguageKeyRegisterDto TRegister)
    {
        var languageKeys = new List<LanguageKey>();
        var keys = TRegister.key.Trim().Split(" ");
        foreach (var key in keys)
        {
            var entity = _mapper.Map<LanguageKey>(TRegister);
            entity.key = key;
            LogRowRegister(ref entity);
            languageKeys.Add(entity);
        }
        _repo.AddRange(languageKeys);
        await _repo.SaveAllAsync();
    }
    public override void Filter(ref IQueryable<LanguageKey> entities, PaginationParam paginationParam)
    {
        if (!string.IsNullOrEmpty(paginationParam.filterValue))
            entities = entities.Where(a => a.key.Contains(paginationParam.filterValue) || a.ScreenApp.NameAr.Contains(paginationParam.filterValue) || a.ScreenApp.NameEn.Contains(paginationParam.filterValue));
    }
    public override void Sort(ref IQueryable<LanguageKey> entities, PaginationParam paginationParam)
    {
        if ((!string.IsNullOrEmpty(paginationParam.filterType)) && (!string.IsNullOrEmpty(paginationParam.sortType)))
        {
           switch (paginationParam.filterType)
            {
                case "key": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.key) : entities.OrderByDescending(a => a.key); break;
                case "screenAppNameAr": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.ScreenApp.NameAr) : entities.OrderByDescending(a => a.ScreenApp.NameAr); break;
                case "screenAppNameEn": entities = paginationParam.sortType == "asc" ? entities.OrderBy(a => a.ScreenApp.NameEn) : entities.OrderByDescending(a => a.ScreenApp.NameEn); break;
                default: entities = entities.OrderBy(a => a.Id); break;
             }
        }
    }


}

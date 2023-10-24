
using Application.Common.Pagination;
using AutoMapper;
using Core.Interfaces;
using Core.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using Application.Business.Common;
using Application.IBusiness.Common;
using Application.Dtos.Message;
using FluentValidation;
using Application.Services;
using Application.IBusiness.Localization;
using Core.Entities.Management;
using Application.Dtos.Localization.Language;
using Microsoft.EntityFrameworkCore;
using Core.Exceptions;

namespace Application.Business.Localization;
public class LanguageBusiness
: EntitiesBusinessCommon<
Language,
LanguageGetDto,
LanguageRegisterDto,
LanguageEditDto,
PaginationParam
>, ILanguageBusiness
{
     public LanguageBusiness(
         IRepositoryApp<Language> Repo,
         IMapper mapper,
         IHttpContextAccessor accssor,
         ILogCustom LogCustom,
         IStringLocalizerCustom localizer,
         IRepositoryMessage iRepositoryMessage,
          IClockService iClockService
          ) : base(Repo, mapper, accssor, LogCustom, localizer, iRepositoryMessage,iClockService)
    {    }
    public override async Task Register(LanguageRegisterDto TRegister)
    {
        var validationResult = await _repo.SingleOrDefaultAsNoTrackingAsync(a=>a.Name==TRegister.Name|| a.DisplayName==TRegister.DisplayName);
        if (validationResult!=null)
            throw new ExceptionCommonReponse(MessageReturn.Localization_NameFound, 400);
        var entity = _mapper.Map<Language>(TRegister);
        LogRowRegister(ref entity);
        _repo.Add(entity); 
        await _repo.SaveAllAsync();
     }
       public   async Task<List<LanguageListDto>> GetAllListLanguage()
    {
        var repo = _repo.GetAll(a=>a.IsActive);
        var entities = await _mapper.ProjectTo<LanguageListDto>(repo).ToListAsync();
        return entities;
    }

   

    public override void Filter(ref IQueryable<Language> entities, PaginationParam paginationParam) {
        if(!string.IsNullOrEmpty(paginationParam.filterValue))
       entities = entities.Where(a => 
        a.Name.Contains(paginationParam.filterValue)
        ||a.DisplayName.Contains(paginationParam.filterValue)
         );
     }
    public async    Task  SetAsDefault(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
         entity.IsDefault=true;
        // 
        var entitiyDefault=await _repo.SingleOrDefaultAsync(a=>a.IsDefault==true);
        if(entitiyDefault!=null){
        entitiyDefault.IsDefault=false;
        LogRowEdit(ref entitiyDefault);
        _repo.Update(entitiyDefault);
        }
        LogRowEdit(ref entity);
        _repo.Update(entity);
        await _repo.SaveAllAsync();
 
  }
}

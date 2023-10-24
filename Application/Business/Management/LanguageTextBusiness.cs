using Application.Business.Common;
using Application.Dtos.Localization.LanguageText;
using Application.IBusiness.Common;
using Application.IBusiness.Localization;
using Application.IRpository.Managment;
using AutoMapper;
using Core.Entities.Management;
using Core.Interfaces;
using Core.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace Application.Business.Localization;
public class LanguageTextBusiness
: EntitiesBusinessCommon<
LanguageText,
LanguageTextGetDto,
LanguageTextRegisterDto,
LanguageTextEditDto,
LanguageTextParam
>, ILanguageTextBusiness
{
    private readonly IRepositoryApp<Language> _repoLanguage;
    private readonly IRepoLanguageText _iRepoLanguageText;
    private readonly IRepositoryApp<LanguageKey> _repoLanguageKey;
    private readonly IRepositoryApp<ModuleApp> _moduleApp;
    private readonly IRepositoryApp<LanguageKeysView> _languageTextLocalView;

    public LanguageTextBusiness(
        IRepoLanguageText IRepoLanguageText,
        IMapper mapper,
        IHttpContextAccessor accssor,
        ILogCustom LogCustom,
        IStringLocalizerCustom localizer,
        IRepositoryMessage iRepositoryMessage,
         IClockService iClockService,
         IRepositoryApp<LanguageKey> repoLanguageKey,
         IRepositoryApp<ModuleApp> ModuleApp,
         IRepositoryApp<LanguageKeysView> LanguageTextLocalView,

         IRepositoryApp<Language> repoLanguage) : base(IRepoLanguageText, mapper, accssor, LogCustom, localizer, iRepositoryMessage, iClockService)
    {
        _iRepoLanguageText = IRepoLanguageText;
        _repoLanguageKey = repoLanguageKey;
        _moduleApp = ModuleApp;
        _languageTextLocalView = LanguageTextLocalView;
        _repoLanguage = repoLanguage;
    }
    public virtual async Task UpdateToLanguage()
    {
        List<LanguageText> entities = new List<LanguageText>();
        var languagesKeys = await _repoLanguageKey.GetAll().AsNoTracking().Select(a => a.Id).ToListAsync();
        var languages = await _repoLanguage.GetAll().AsNoTracking().Select(a => a.Id).ToListAsync();
        foreach (var langId in languages)
        {
            foreach (var keyId in languagesKeys)
            {
                var entityText = await _repo.Count(a => a.LanguageId == langId && a.LanguageKeyId == keyId);
                if (entityText <= 0)
                {
                    var entity = new LanguageText { LanguageId = langId, LanguageKeyId = keyId, TargetValue = string.Empty };
                    LogRowRegister(ref entity);
                    entities.Add(entity);
                }
            }
        }
        if (entities.Any())
        {
            _repo.AddRange(entities);
            await _repo.SaveAllAsync();
        }
    }
    public async Task<Dictionary<string, Dictionary<string, Dictionary<string, string>>>> GetLanguageForLocal(string langName)
    {
        var texts = await  _languageTextLocalView.GetAllAsync(a=>a.Name==langName);
        var textsG = texts.GroupBy(a => a.ModuleKey);
        var result = textsG.ToDictionary(a => a.Key, s => s.ToList().GroupBy(a => a.ScreenKey).ToDictionary(a => a.Key, s => s.ToDictionary(a => a.LanguageKey, p => p.TargetValue)));
        return result;
    }
    public override void Filter(ref IQueryable<LanguageText> entities, LanguageTextParam paginationParam)
    {
         
            if (!string.IsNullOrEmpty(paginationParam.TargetValue))
                entities = entities.Where(a => a.TargetValue.Contains(paginationParam.TargetValue));
            if (paginationParam.LanguageId != null)
                entities = entities.Where(a => a.LanguageId == (int)paginationParam.LanguageId);
            if (paginationParam.LanguageKeyId != null)
                entities = entities.Where(a => a.LanguageId == (int)paginationParam.LanguageKeyId);
            if (paginationParam.ScreenId != null && paginationParam.ScreenId.Any())
                entities = entities.Where(a => paginationParam.ScreenId.Contains(a.LanguageKey.ScreenAppId));
        
    }
}

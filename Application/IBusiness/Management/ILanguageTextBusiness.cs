using System;
using Application.Common.Pagination;
using Application.Dtos.Localization.LanguageText;
using Application.Dtos.Message;
using Application.IBusiness.Common;
using Core.Entities.Management;
namespace Application.IBusiness.Localization;
public interface ILanguageTextBusiness : IEntitiesBusinessCommon<
LanguageText,
LanguageTextGetDto,
LanguageTextRegisterDto,
LanguageTextEditDto,
LanguageTextParam>
{
Task UpdateToLanguage();
Task<Dictionary<string, Dictionary<string, Dictionary<string, string>>> > GetLanguageForLocal(string langName);

}
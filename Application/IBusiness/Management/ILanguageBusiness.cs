using Application.Common.Pagination;
using Application.Dtos.Localization.Language;
using Application.Dtos.Message;
using Application.IBusiness.Common;
using Core.Entities.Management;
namespace Application.IBusiness.Localization;
public interface ILanguageBusiness : IEntitiesBusinessCommon<
Language,
LanguageGetDto,
LanguageRegisterDto,
LanguageEditDto,
PaginationParam>
{
    Task<List<LanguageListDto>> GetAllListLanguage();
    Task SetAsDefault(int id);
}

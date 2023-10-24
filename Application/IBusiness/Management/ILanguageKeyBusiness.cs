using Application.Common.Pagination;
using Application.Dtos.Localization.LanguageKey;
using Application.Dtos.Message;
using Application.IBusiness.Common;
using Core.Entities.Management;

namespace Application.IBusiness.Localization;
public interface ILanguageKeyBusiness : IEntitiesBusinessCommon<
LanguageKey,
LanguageKeyGetDto,
LanguageKeyRegisterDto,
LanguageKeyEditDto,
PaginationParam>
{
}

using Core.Common;
namespace Application.Dtos.Localization.LanguageKey;
public class LanguageKeyEditDto : LanguageKeyRegisterDto, IBaseId
{
    public int Id { get; set; }
}

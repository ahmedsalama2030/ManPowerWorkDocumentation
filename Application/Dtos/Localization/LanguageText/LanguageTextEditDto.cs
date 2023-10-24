using Core.Common;

namespace Application.Dtos.Localization.LanguageText;
public class LanguageTextEditDto:LanguageTextRegisterDto, IBaseId
{
    public int Id { get; set; }
}

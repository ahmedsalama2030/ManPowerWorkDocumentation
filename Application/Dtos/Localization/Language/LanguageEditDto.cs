using Core.Common;
namespace Application.Dtos.Localization.Language;
public class LanguageEditDto : LanguageRegisterDto, IBaseId
{
    public int Id { get; set; }
}

using Core.Common;
namespace Application.Dtos.Localization.LanguageKey;
public class LanguageKeyGetDto : BaseEntityGetTrace
{
    public string key { get; set; }
    public int ScreenAppId { get; set; }
    public string ScreenAppNameEn { get; set; }
    public string ScreenAppNameAr { get; set; }
}

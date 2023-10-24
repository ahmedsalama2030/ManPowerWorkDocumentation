using Core.Common;

namespace Application.Dtos.Localization.Language;
public class LanguageGetDto : BaseEntityGetTrace
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Icon { get; set; }
    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
    public  string Direction { get; set; }

}

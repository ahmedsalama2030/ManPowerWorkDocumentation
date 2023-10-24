using Core.Common;

namespace Application.Dtos.Localization.Language;
public class LanguageListDto:IBaseId
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Icon { get; set; }
    public  string Direction { get; set; }
    public bool IsDefault { get; set; }


}

using Core.Common;
namespace Application.Dtos.Auth.ScreenApp;
public class ScreenAppGetDto : BaseEntityGetTrace
{
    public string NameEn { get; set; }
    public string NameAr { get; set; }
    public string Key { get; set; }
    public string UrlPage { get; set; }
    public string UrlDoc { get; set; }
    public string Description { get; set; }
    public int ModuleAppId { get; set; }
    public string ModuleAppNameEn { get; set; }
    public string ModuleAppNameAr { get; set; }
  public bool IsMain { get; set; }
  public bool IsShowPermission { get; set; }


}

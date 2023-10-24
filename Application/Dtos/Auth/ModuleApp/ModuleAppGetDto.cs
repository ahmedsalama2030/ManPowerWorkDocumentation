using Core.Common;
namespace Application.Dtos.Auth.ModuleApp;
public class ModuleAppGetDto : BaseEntityGetTrace
{
    public string NameEn { get; set; }
    public string NameAr { get; set; }
  public string Key { get; set; }
  public bool IsMain { get; set; }


}

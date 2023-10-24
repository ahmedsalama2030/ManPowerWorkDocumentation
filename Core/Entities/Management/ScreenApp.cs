using System.Collections.Generic;
using Core.Common;
namespace Core.Entities.Management;
public class ScreenApp: BaseEntity
{
  public string NameEn { get; set; }
  public string NameAr { get; set; }
  public string Key { get; set; }
  public string UrlPage { get; set; }
  public string UrlDoc { get; set; }
  public string Description { get; set; }
  public int ModuleAppId { get; set; }
  public bool IsMain { get; set; }
  public bool IsShowPermission { get; set; }

  public ModuleApp ModuleApp { get; set; }
  public List <ClaimApp> ClaimsApp { get; set; } 
  public List <LanguageKey> LanguageKeys { get; set; } 

}

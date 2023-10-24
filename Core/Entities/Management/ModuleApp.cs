using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Common;
namespace Core.Entities.Management;
public class ModuleApp: BaseEntity
{
  public string NameEn { get; set; }
  public string NameAr { get; set; }
  public string Key { get; set; }
  public bool IsMain { get; set; }
  public List <ScreenApp> ScreenApps { get; set; } 

}

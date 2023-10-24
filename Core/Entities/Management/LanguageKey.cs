using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Common;
 
namespace Core.Entities.Management;
public class LanguageKey : BaseEntity
{
    public string key { get; set; }
    public int ScreenAppId { get; set; }
    public ScreenApp ScreenApp { get; set; }
    public List<LanguageText> LanguageTexts { get; set; }

}

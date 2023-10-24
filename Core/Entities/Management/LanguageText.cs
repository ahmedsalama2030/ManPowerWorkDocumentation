using System.ComponentModel.DataAnnotations.Schema;
using Core.Common;
namespace Core.Entities.Management;
public class LanguageText : BaseEntity
{
    public string TargetValue { get; set; }
    public int LanguageId { get; set; }
    public int LanguageKeyId { get; set; }
    public Language Language { get; set; }
    public LanguageKey LanguageKey { get; set; }
}

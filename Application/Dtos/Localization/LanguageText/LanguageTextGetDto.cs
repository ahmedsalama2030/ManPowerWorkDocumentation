using System;
using Core.Common;
namespace Application.Dtos.Localization.LanguageText;
public class LanguageTextGetDto : BaseEntityGetTrace
{
    public string TargetValue { get; set; }
    public int LanguageId { get; set; }
    public int LanguageKeyId { get; set; }
    public string LanguageName { get; set; }
    public string LanguageDisplayName { get; set; }
    public string LanguageKey { get; set; }
    

}


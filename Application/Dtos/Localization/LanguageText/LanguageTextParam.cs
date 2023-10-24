using Application.Common.Pagination;

namespace Application.Dtos.Localization.LanguageText;
public class LanguageTextParam:PaginationParam
{
    public string TargetValue { get; set; }
    public int? LanguageId { get; set; }
    public int? LanguageKeyId { get; set; }
    public int?[] ScreenId { get; set; }
}

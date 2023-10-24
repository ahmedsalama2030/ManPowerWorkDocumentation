using Core.Common;

namespace Application.Dtos.Auth.ClaimApp;
public class ClaimAppGetDto : BaseEntityGetTrace
{
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }
    public string Key { get; set; }
    public int ScreenAppId { get; set; }
    public string ScreenAppNameAr { get; set; }
    public string ScreenAppNameEn { get; set; }
}
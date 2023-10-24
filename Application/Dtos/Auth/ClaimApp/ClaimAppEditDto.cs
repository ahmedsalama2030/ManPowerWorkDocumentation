using Core.Common;

namespace Application.Dtos.Auth.ClaimApp;
public class ClaimAppEditDto 
{
    public int? Id { get; set; }
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }
    public string Key { get; set; }
    public int ScreenAppId { get; set; }
    public bool IsSelected { get; set; }=false;
}

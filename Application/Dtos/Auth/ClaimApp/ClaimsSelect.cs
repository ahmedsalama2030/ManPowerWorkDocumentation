using System;

namespace Application.Dtos.Auth.ClaimApp;
public class ClaimsSelect
{
    public string ModuleAppNameEn { get; set; }
    public string ModuleAppNameAr { get; set; }
    public int ModuleAppId { get; set; }
    public HashSet <ScreenClaim> ScreenClaims { get; set; }=new HashSet<ScreenClaim>();


}


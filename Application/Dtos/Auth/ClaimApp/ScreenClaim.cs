using System;

namespace Application.Dtos.Auth.ClaimApp;
public class ScreenClaim
{
    public int ScreenAppId { get; set; }
    public string ScreenAppNameEn { get; set; }
    public string ScreenAppNameAr { get; set; }
     public HashSet <ClaimAppEditDto> claims { get; set; }=new HashSet<ClaimAppEditDto>();


}


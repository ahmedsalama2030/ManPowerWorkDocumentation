using System.ComponentModel.DataAnnotations.Schema;
using Core.Common;
using Core.Entities.Management;

namespace Core.Entities.Management;
public class ClaimApp: BaseEntity
{
    public  string ClaimType { get; set; }
    public  string ClaimValue { get; set; }
    public string Key { get; set; }
    public int ScreenAppId { get; set; }
    public ScreenApp ScreenApp { get; set; }
}

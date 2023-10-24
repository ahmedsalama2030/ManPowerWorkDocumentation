using System.ComponentModel.DataAnnotations.Schema;
using Core.Common;

namespace Core.Entities.Management;
public class Language : BaseEntity
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Icon { get; set; }
    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
    public  string Direction { get; set; }
}

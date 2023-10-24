using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Common;
using Microsoft.AspNetCore.Identity;
namespace Core.Entities.Management;
public class Role : IdentityRole<int>,IBaseEntity
{
    public bool IsDeleted { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public IList<UserRole> UserRole { get; set; }
     public DateTime? LastModificationTime { get; set; }
    public int? LastModifierId { get; set; }
    public DateTime? CreationTime { get; set; }
    public int? CreatorId { get; set; }
    public string IPDeviceCreaded { get; set; }
    public string IPDeviceLastEdit { get; set; }
    public Guid? ClientId { get; set; }
}

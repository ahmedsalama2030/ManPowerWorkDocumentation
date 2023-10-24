using System;
using System.Collections.Generic;
using Core.Common;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
namespace Core.Entities.Management;
public class User : IdentityUser<int>, IBaseEntity
{
    public DateTime? LastModificationTime { get; set; }
    public int? LastModifierId { get; set; }
    public DateTime? CreationTime { get; set; }
    public int? CreatorId { get; set; }
    public DateTime? LastLogin { get; set; }
    public string IPDeviceCreaded { get; set; }
    public string IPDeviceLastEdit { get; set; }
    public bool IsDeleted { get; set; }
    public UserType UserType { get; set; }
    public AccountType AccountType { get; set; }

    public Guid? ClientId { get; set; }
    public List<UserRole> UserRole { get; set; }
    public int? UserTenantId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string WorkEmail { get; set; }
    public string FaxNumber { get; set; }
    public string LogoPath { get; set; }
 
}

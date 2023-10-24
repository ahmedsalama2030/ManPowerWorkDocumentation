using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Common;
public class BaseEntity : BaseId, IBaseEntity
{
    public DateTime? LastModificationTime { get; set; }
    public int? LastModifierId { get; set; }
    public DateTime? CreationTime { get; set; }
    public int? CreatorId { get; set; }
    public string IPDeviceCreaded { get; set; }
    public string IPDeviceLastEdit { get; set; }
    public bool IsDeleted { get; set; }
    public Guid? ClientId { get; set; }  // forign key for index
}


using System;

namespace Core.Common;
public interface IBaseEntity:IBaseId
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



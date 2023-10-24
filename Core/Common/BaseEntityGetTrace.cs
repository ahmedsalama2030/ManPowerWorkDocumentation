using System;
namespace Core.Common;
public class BaseEntityGetTrace : BaseId
{
  public DateTime? LastModificationTime { get; set; }
  public DateTime? CreationTime { get; set; }
  public Guid? ClientId { get; set; }  // forign key for index
}

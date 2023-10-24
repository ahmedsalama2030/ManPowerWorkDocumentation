using System;
using Core.Common;
using Core.Entities.Management;
namespace Core.Entities.Chat;
public class Chat : BaseEntity
{
    public int SenderId { get; set; }
     public User Sender { get; set; }
    public int RecipientId { get; set; }
    public User Recipient { get; set; }
    public string Content { get; set; }
    public string Name { get; set; }
    public string Size { get; set; }
    public string MediaType { get; set; }
    public string ContentType { get; set; }
    public bool IsRead { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; }
    public bool SenderDeleted { get; set; }
    public bool RecipientDeleted { get; set; }
}

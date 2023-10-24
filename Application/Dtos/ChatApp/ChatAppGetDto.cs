using System;
using Core.Common;

namespace Application.Dtos.ChatApp;
public class ChatAppGetDto : IBaseId
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int RecipientId { get; set; }
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


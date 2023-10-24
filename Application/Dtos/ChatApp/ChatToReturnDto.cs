using System;

namespace Application.Dtos.ChatApp;
public class ChatToReturnDto
{
    public int SenderId { get; set; }
    public int RecipientId { get; set; }
    public string Content { get; set; }
    public string Name { get; set; }
    public string Size { get; set; }
    public string MediaType { get; set; }
    public string ContentType { get; set; }
    public bool IsRead { get; set; }
    public DateTime MessageSent { get; set; }
}


using System;

namespace Application.Dtos.ChatApp;
public class ChatRegisterDto
{
    public int SenderId { get; set; }
    public int RecipientId { get; set; }
    public DateTime? MessageSent { get; set; }
    public string Content { get; set; }
}


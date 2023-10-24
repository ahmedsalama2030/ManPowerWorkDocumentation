using System;
using Core.Entities.Management;

namespace Application.Dtos.ChatApp;
public class ChatUnRead
{
    public int NumberMessage { get; set; }
    public DateTime LastSend { get; set; }
    public string Name { get; set; }
    public int SenderId { get; set; }
}


namespace Application.Dtos.ChatApp;
public class ChatRegisterMoreDto
{
    public int SenderId { get; set; }
    public int[] RecipientsId { get; set; }
    public string Content { get; set; }
}

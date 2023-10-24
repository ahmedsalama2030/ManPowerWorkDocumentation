 
using System.Collections.Concurrent;
using Core.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs;
public class StudyCountHub : Hub
{
    private readonly ConcurrentDictionary<int, HashSet<string>> ConnectedUsers;
    private readonly IChatAppConnection _iChatAppConnection;
    protected readonly IHttpContextAccessor _accssor;


    public StudyCountHub(IChatAppConnection iChatAppConnection, IHttpContextAccessor accssor)
    {
        _iChatAppConnection = iChatAppConnection;
        ConnectedUsers = _iChatAppConnection.GetConnectedUsers(); ;
        _accssor = accssor;
    }

    public async void StudyCount()
    {
                await Clients.All.SendAsync("studyCount");
    }
  
  public override async Task OnConnectedAsync()
    {
        var userId= int.Parse(Context.UserIdentifier);
        if (!ConnectedUsers.ContainsKey(userId))
            ConnectedUsers[userId] = new HashSet<string>();
        ConnectedUsers[userId].Add(Context.ConnectionId);
        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        int userId = int.Parse(Context.UserIdentifier);
        if (ConnectedUsers.ContainsKey(userId))
        {
            ConnectedUsers[userId].Remove(Context.ConnectionId);
            if (ConnectedUsers[userId].Count == 0)
            {
                HashSet<string> x;
                ConnectedUsers.Remove(userId, out x);
            }
        }
        await base.OnDisconnectedAsync(exception);
    }
 

}

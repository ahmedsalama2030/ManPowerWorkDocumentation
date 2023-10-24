using System.Collections.Concurrent;
using Core.Interfaces.Common;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs;
public class ChatHub : Hub
{
    private readonly ConcurrentDictionary<int, HashSet<string>> ConnectedUsers;
    private readonly IChatAppConnection _iChatAppConnection;

    public ChatHub(IChatAppConnection iChatAppConnection)
    {
        _iChatAppConnection = iChatAppConnection;
        ConnectedUsers = _iChatAppConnection.GetConnectedUsers(); ;
    }
    public async void refresh(int userId)
    {
        if (ConnectedUsers.ContainsKey(userId))
        {
            foreach (string connectionId in ConnectedUsers[userId])
                await Clients.Client(connectionId).SendAsync("refresh");
        }
    }
    public async void studyStatus(int userId)
    {
        if (ConnectedUsers.ContainsKey(userId))
        {
            foreach (string connectionId in ConnectedUsers[userId])
                await Clients.Client(connectionId).SendAsync("refresh");
        }
    }
    public async void refreshMore(int[] usersId)
    {
        foreach (var id in usersId)
        {
         if (ConnectedUsers.ContainsKey(id))
            {
                foreach (string connectionId in ConnectedUsers[id])
                    await Clients.Client(connectionId).SendAsync("refresh");
            }
        }
    }
    public override async Task OnConnectedAsync()
    {
        int userId = int.Parse(Context.UserIdentifier);

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

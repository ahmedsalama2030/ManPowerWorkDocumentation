using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Core.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;

namespace Application.Hubs;
public class DoctorAssignHub : Hub
{
    private readonly ConcurrentDictionary<int, HashSet<string>> ConnectedUsers;
    private readonly IChatAppConnection _iChatAppConnection;
    protected readonly IHttpContextAccessor _accssor;


    public DoctorAssignHub(IChatAppConnection iChatAppConnection, IHttpContextAccessor accssor)
    {
        _iChatAppConnection = iChatAppConnection;
        ConnectedUsers = _iChatAppConnection.GetConnectedUsers(); ;
        _accssor = accssor;
    }

    public async void DoctorAssign(int userId)
    {
        if (ConnectedUsers.ContainsKey(userId))
        {
            foreach (string connectionId in ConnectedUsers[userId])
                await Clients.Client(connectionId).SendAsync("doctorAssign");
        }
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



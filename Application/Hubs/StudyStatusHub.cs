 
using System.Collections.Concurrent;
using System.Security.Claims;
using Core.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs;
public class StudyStatusHub : Hub
{
    private readonly ConcurrentDictionary<int, HashSet<string>> ConnectedUsers;
    private readonly IChatAppConnection _iChatAppConnection;
    protected readonly IHttpContextAccessor _accssor;


    public StudyStatusHub(IChatAppConnection iChatAppConnection, IHttpContextAccessor accssor)
    {
        _iChatAppConnection = iChatAppConnection;
        ConnectedUsers = _iChatAppConnection.GetConnectedUsers(); ;
        _accssor = accssor;
    }

    public async void StudyStatus(int userId)
    {
        if (ConnectedUsers.ContainsKey(userId))
        {
            foreach (string connectionId in ConnectedUsers[userId])
                await Clients.Client(connectionId).SendAsync("studyStatus");
        }
    }
    public async void StudyStatusMore(int[] usersId)
    {
        foreach (var id in usersId)
        {
         if (ConnectedUsers.ContainsKey(id))
            {
                foreach (string connectionId in ConnectedUsers[id])
                    await Clients.Client(connectionId).SendAsync("studyStatus");
            }
        }
    }
    public override async Task OnConnectedAsync()
    {
        var tenantId= int.Parse(Context.User.FindFirstValue("TenantId"));
        if (!ConnectedUsers.ContainsKey(tenantId))
            ConnectedUsers[tenantId] = new HashSet<string>();
        ConnectedUsers[tenantId].Add(Context.ConnectionId);
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

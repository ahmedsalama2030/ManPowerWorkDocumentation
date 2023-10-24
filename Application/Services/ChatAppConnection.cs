using System;
using System.Collections.Concurrent;
using Core.Interfaces.Common;

namespace Application.Services;
public class ChatAppConnection : IChatAppConnection
{
    ConcurrentDictionary<int, HashSet<string>> ConnectedUsers;
    public ConcurrentDictionary<int, HashSet<string>> GetConnectedUsers()
    {
        ConnectedUsers ??= new ConcurrentDictionary<int, HashSet<string>>();
        return ConnectedUsers;

    }
    public void RemoveUser(int id)
    {
        if (ConnectedUsers != null && ConnectedUsers.ContainsKey(id))
        {
            HashSet<string> x;
            ConnectedUsers.TryRemove(id, out x);

        }
    }
}


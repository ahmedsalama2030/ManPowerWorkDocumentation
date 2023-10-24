using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Core.Interfaces.Common;
public interface IChatAppConnection
{
 ConcurrentDictionary<int, HashSet<string>> GetConnectedUsers () ;
 void   RemoveUser (int id);
}


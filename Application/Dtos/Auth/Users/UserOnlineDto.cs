using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Common;

namespace Application.Dtos.Auth.Users;
public class UserOnlineDto : BaseId
{
    public string Name { get; set; }
    public bool IsOnline { get; set; }
}

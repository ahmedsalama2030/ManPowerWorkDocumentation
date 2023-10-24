using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.Auth.Users;
using Core.Common;
using Core.Enums;

namespace Application.Dtos.Users;
public class UserGetDto : IBaseId
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string JobTitle { get; set; }
    public string PhoneNumber { get; set; }
    public string RoleName { get; set; }
    public DateTime? LastLogin { get; set; }
   public UserType UserType { get; set; }


}


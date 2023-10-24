 
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Common;
using Microsoft.AspNetCore.Identity;
namespace Core.Entities.Management;
[Table("UserLogins")]
public class UserLogin :IdentityUserLogin<int>
{
 
}

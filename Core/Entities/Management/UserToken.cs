 using System.ComponentModel.DataAnnotations.Schema;
 using Microsoft.AspNetCore.Identity;
namespace Core.Entities.Management;
[Table("UsersRoles")]
public class UserToken :IdentityUserToken<int>
{
 
}

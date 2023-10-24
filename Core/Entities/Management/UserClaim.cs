using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Common;
using Microsoft.AspNetCore.Identity;
namespace Core.Entities.Management;
[Table("UserClaims")]
public class UserClaim :IdentityUserClaim<int>,IBaseId
{
 
}

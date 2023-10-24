using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Common;
using Microsoft.AspNetCore.Identity;
namespace Core.Entities.Management;
public class RoleClaim :IdentityRoleClaim<int>,IBaseId
{
 
}

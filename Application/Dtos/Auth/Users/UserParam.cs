using Application.Common.Pagination;

namespace Application.Dtos.Auth.Users;
public class UserParam : PaginationParam
{
 
public bool IsLocked { get; set; }
public bool IsAllUsers { get; set; }
public bool IsActiveUsers { get; set; }
public bool IsDeletedUsers { get; set; }
}

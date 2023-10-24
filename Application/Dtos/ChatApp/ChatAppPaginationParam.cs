using Application.Common.Pagination;

namespace Application.Dtos.ChatApp;
public class ChatAppPaginationParam : PaginationParam
{
    public int? UserId { get; set; }
    public int? SenderId { get; set; }
}

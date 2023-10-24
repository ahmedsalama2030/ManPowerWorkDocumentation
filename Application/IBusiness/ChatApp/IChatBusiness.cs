using Application.Common.Pagination;
using Application.Dtos.ChatApp;
using Application.IBusiness.Common;
using Core.Entities.Chat;
using Microsoft.AspNetCore.Http;

namespace Application.IBusiness.ChatApp;
public interface IChatBusiness : IEntitiesBusinessCommon<
Chat,
ChatAppGetDto,
Chat,
Chat,
ChatAppPaginationParam>
{
    Task<IEnumerable<ChatUnRead>> GetChatUnread(HttpResponse Response, ChatAppPaginationParam paginationParam);
    Task<double> GetUnreadChatForUser(int userId);
    Task MarkeRead(ChatMarkReadDto chatMarkReadDto);
    Task<ChatToReturnDto> RegisterOne(HttpRequest Request, ChatRegisterDto chatRegisterDto);
    FileStream GetChatMedia(string path);
     Task<ChatToReturnDto> RegisterMore(HttpRequest Request, ChatRegisterMoreDto chatRegisterDto);
}

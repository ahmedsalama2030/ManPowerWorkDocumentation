using Application.Business.Common;
using Application.Common.Pagination;
using Application.Dtos.ChatApp;
using Application.Dtos.Common;
using Application.Enums;
using Application.Helper.ExtentionMethod;
using Application.Hepler.ExtensionsMethod;
using Application.IBusiness.ChatApp;
using Application.IBusiness.Common;
using Application.Services;
using AutoMapper;
using Core.Entities.Chat;
using Core.Interfaces;
using Core.Interfaces.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Business.ChatApp;
public class ChatBusiness : EntitiesBusinessCommon<
Chat,
ChatAppGetDto,
Chat,
Chat,
ChatAppPaginationParam
>, IChatBusiness
{
    private readonly IWebHostEnvironment _ihostingEnvironment;
    private readonly IConfiguration _config;

    public ChatBusiness(
          IRepositoryApp<Chat> Repo,
          IMapper mapper,
          IHttpContextAccessor accssor,
          ILogCustom LogCustom,
          IStringLocalizerCustom localizer,
          IRepositoryMessage iRepositoryMessage,
           IClockService iClockService,
           IWebHostEnvironment ihostingEnvironment,
           IConfiguration config

           ) : base(Repo, mapper, accssor, LogCustom, localizer, iRepositoryMessage, iClockService)
    {
        _ihostingEnvironment = ihostingEnvironment;
        _config = config;
    }
    public override async Task<List<ChatAppGetDto>> Get(HttpResponse Response, ChatAppPaginationParam paginationParam)
    {
        var entities = _repo.GetAll(m => m.RecipientId == paginationParam.UserId && m.SenderId == paginationParam.SenderId && m.RecipientDeleted == false || m.RecipientId == paginationParam.SenderId && m.SenderId == paginationParam.UserId && m.SenderDeleted == false,
           s => s.Sender, r => r.Recipient);
        Filter(ref entities, paginationParam);
        Sort(ref entities, paginationParam);
        var entitiesMapped = _mapper.ProjectTo<ChatAppGetDto>(entities);
        var PagedList = await PagedList<ChatAppGetDto>.CreateAsync(entitiesMapped, paginationParam.pageNumber, paginationParam.PageSize);
        Response.AddPagination(PagedList.TotalItems, PagedList.TotalPages);
        object paramFilter = string.IsNullOrEmpty(paginationParam.filterValue) ? null : paginationParam;
        _logger.Info<Chat>(MessageReturn.Common_SearchFor, paramFilter);
        return PagedList.ToList();
    }

    public FileStream GetChatMedia(string name)
    {
        var paths = _config.GetSection("AppSettings:PhysicalChatPath").Value;

        var imageFileStream = new FileStream(paths + name, FileMode.Open, FileAccess.Read);

        return imageFileStream;
    }

    public async Task<IEnumerable<ChatUnRead>> GetChatUnread(HttpResponse Response, ChatAppPaginationParam paginationParam)
    {
        var messages = _repo.GetAll(p => p.RecipientId == paginationParam.UserId && p.IsRead == false, s => s.Sender);
        var newMessages = (await messages.OrderByDescending(d => d.MessageSent).ToListAsync()).GroupBy(s => s.SenderId)
        .Select(
            a => new ChatUnRead
            {
                LastSend = a.FirstOrDefault().MessageSent,
                NumberMessage = a.Count(),
                Name = a.FirstOrDefault().Sender.Name,
                SenderId = a.FirstOrDefault().Sender.Id
            });
        var PagedList = PagedList<ChatUnRead>.CreateAsync(newMessages, paginationParam.pageNumber, paginationParam.PageSize);
        Response.AddPagination(PagedList.TotalItems, PagedList.TotalPages);
        return PagedList.ToList();
    }

    public async Task<double> GetUnreadChatForUser(int userId)
    {
        var count = await _repo.Count(m => m.IsRead == false && m.RecipientId == userId);
        return count;
    }

    public async Task MarkeRead(ChatMarkReadDto chatMarkReadDto)
    {
        var chats = await _repo.GetAllAsync(a => chatMarkReadDto.MessagesId.Contains(a.Id) && a.RecipientId == chatMarkReadDto.UserId && a.IsRead == false);
        if (chats.Any())
        {
            foreach (var chat in chats){
                chat.IsRead = true;
               chat.DateRead=_iClockService.Now;
            }
            _repo.UpdateRange(chats);
            await _repo.SaveAllAsync();
        }
    }

    public async Task<ChatToReturnDto> RegisterOne(HttpRequest Request, ChatRegisterDto chatRegisterDto)
    {
        var message = _mapper.Map<Chat>(chatRegisterDto);
        var formCollection = await Request.ReadFormAsync();
        if (formCollection.Files.Count() > 0)
        {
            var attachment = formCollection.Files[0];
            ImagesSave media = await attachment.SaveImageOnDisk(_config.GetSection("AppSettings:PhysicalChatPath").Value, _config.GetSection("AppSettings:ServerChatPath").Value);
            message.Content = media.path;
            message.Name = media.name;
            message.Size = media.size;
            message.MediaType = MediaType.media.ToString();
            message.ContentType = media.contentType;
        }
        else
            message.MediaType = MediaType.text.ToString();
        LogRowRegister(ref message);
        message.MessageSent = _iClockService.Now;
        _repo.Add(message);
        await _repo.SaveAllAsync();
        return _mapper.Map<ChatToReturnDto>(message);


    }
    public async Task<ChatToReturnDto> RegisterMore(HttpRequest Request, ChatRegisterMoreDto chatRegisterDto)
    {
        var message = _mapper.Map<Chat>(chatRegisterDto);
        var formCollection = await Request.ReadFormAsync();
        if (formCollection.Files.Count() > 0)
        {
            var attachment = formCollection.Files[0];
            ImagesSave media = await attachment.SaveImageOnDisk(_config.GetSection("AppSettings:PhysicalChatPath").Value, _config.GetSection("AppSettings:ServerChatPath").Value);
            message.Content = media.path;
            message.Name = media.name;
            message.Size = media.size;
            message.MediaType = MediaType.media.ToString();
            message.ContentType = media.contentType;
        }
        else
            message.MediaType = MediaType.text.ToString();
        LogRowRegister(ref message);
        message.MessageSent = _iClockService.Now;
        var chats = new List<Chat>();
        foreach (var recipientId in chatRegisterDto.RecipientsId)
        {
            message.RecipientId = recipientId;
            chats.Add(message);
        }
        _repo.AddRange(chats);
        await _repo.SaveAllAsync();
        chats=null;
        return _mapper.Map<ChatToReturnDto>(message);
     }

}
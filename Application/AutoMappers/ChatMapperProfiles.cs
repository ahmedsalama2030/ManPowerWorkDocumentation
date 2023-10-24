
 using Application.Dtos.ChatApp;
using AutoMapper;
using Core.Common.Dto;
using Core.Entities.Chat;
namespace Application.Automappers;
public class ChatMapperProfiles : Profile
{
    public ChatMapperProfiles()
    {
        // Hospital
        CreateMap<ChatRegisterDto, Chat>();
        CreateMap<ChatRegisterMoreDto, Chat>();
        CreateMap<Chat, Chat>();
        CreateMap<Chat, ChatToReturnDto>();
        CreateMap<Chat, ChatAppGetDto>();
    }
}

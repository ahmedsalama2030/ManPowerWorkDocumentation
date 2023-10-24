using Application.Dtos.Auth.Audit;
using Application.Dtos.Auth.ClaimApp;
using Application.Dtos.Auth.ModuleApp;
using Application.Dtos.Auth.RoleClaim;
using Application.Dtos.Auth.ScreenApp;
using Application.Dtos.Auth.Users;
using Application.Dtos.Localization.Language;
using Application.Dtos.Localization.LanguageKey;
using Application.Dtos.Localization.LanguageText;
using Application.Dtos.roles;
using Application.Dtos.Users;
using AutoMapper;
using Core.Common.Dto;
 using Core.Entities.Management;

namespace Application.Automappers;
public class ManagementMapperProfiles : Profile
{
    public ManagementMapperProfiles()
    {
        // users
        CreateMap<User, UserListDto>().
        ForMember(d => d.Roles, op => op.MapFrom(t => t.UserRole.FirstOrDefault().Role.Name)).
        ForMember(d => d.UserType, op => op.MapFrom(t => t.UserType.ToString()));
         CreateMap<User, BaseListDto>() ;

        CreateMap<UserLoginDto, User>();
        CreateMap<MarketerRegisterDto, User>();

        CreateMap<UserRegisterDto, User>().ReverseMap();
        CreateMap<UserEditDto, User>().ReverseMap();
        CreateMap<User, UserGetDto>().ForMember(d => d.RoleName, op => op.MapFrom(t => t.UserRole.FirstOrDefault().Role.Name));
 
        // roles  j
        CreateMap<Role, RoleListDto>();
        CreateMap<Role, BaseListDto>();

        CreateMap<RoleRegisterDto, Role>().ReverseMap();
        CreateMap<Role, RoleListEditDto>();
        // 
        CreateMap<UserRoleRegisterDto, UserRole>().ReverseMap();

        // ModuleApp
        CreateMap<ModuleApp, ModuleAppGetDto>();
        CreateMap<ModuleApp, BaseListDto>();

        CreateMap<ModuleAppRegisterDto, ModuleApp>();
        CreateMap<ModuleApp, ModuleAppEditDto>();
        CreateMap<ModuleApp, ModuleAppLocalDto>();

        // RoleClaim
        CreateMap<RoleClaim, RoleClaimGetDto>();
        CreateMap<RoleClaimRegisterDto, RoleClaim>();
        CreateMap<RoleClaim, RoleListEditDto>();
         // ClaimApp
        CreateMap<ClaimApp, ClaimAppGetDto>().ReverseMap();
        CreateMap<ClaimsSelect, ClaimAppView>().ReverseMap();
        CreateMap<ScreenClaim, ClaimAppView>().ReverseMap();
        CreateMap<ClaimAppEditDto, ClaimAppView>().ReverseMap();
        CreateMap<ClaimAppEditDto,ClaimApp >().ReverseMap();
        // ScreenApp
        CreateMap<ScreenApp, ScreenAppGetDto>();
        CreateMap<ScreenApp, BaseListDto>();
        CreateMap<ScreenAppRegisterDto, ScreenApp>();
        CreateMap<ScreenApp, RoleListEditDto>();
        CreateMap<ScreenApp, ScreenAppLocalDto>();
         // Language
        CreateMap<Language, LanguageGetDto>();
        CreateMap<Language, LanguageListDto>();
        CreateMap<Language, BaseListDto>();
        CreateMap<LanguageRegisterDto, Language>();
        CreateMap<LanguageEditDto, Language>();
        // LanguageKeyGetDto
        CreateMap<LanguageKey, LanguageKeyGetDto>();
        CreateMap<LanguageKey, LanguageKeyLocalDto>();

        CreateMap<LanguageKey, BaseListDto>();
        CreateMap<LanguageKeyRegisterDto, LanguageKey>();
        CreateMap<LanguageKeyEditDto, LanguageKey>();
        // LanguageKeyGetDto
        CreateMap<LanguageText, LanguageTextGetDto>().ForMember(d => d.LanguageKey, op => op.MapFrom(t =>t.LanguageKey.key)) ;
        CreateMap<LanguageText, BaseListDto>();
        CreateMap<LanguageTextRegisterDto, LanguageText>();
        CreateMap<LanguageTextEditDto, LanguageText>();
        // Audit
        CreateMap<Audit, AuditGetDto>();




    }
}

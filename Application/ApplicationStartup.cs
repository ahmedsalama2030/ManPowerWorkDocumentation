using Application.Dtos.Auth.ModuleApp;
using Application.Dtos.Auth.ScreenApp;
using Application.IBusiness.Common;
using Application.IBusiness.Localization;
using Application.IBusiness.Management;
using Application.Business.Common;
using Application.Business.Localization;
using Application.Business.Management;
using Application.Services;
using Application.Validations.Auth;
using Core.Interfaces.Common;
using FluentValidation;
using Infrastructure.Business.Management;
using Microsoft.Extensions.DependencyInjection;
using Application.IRpository.Managment;
using Application.Repository.Managment;
using Application.Business.ChatApp;
using Application.IBusiness.ChatApp;
namespace Application;
public static class ApplicationStartup
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    #region  common
    services.AddAutomapperConfigServices();
    services.AddSingleton(typeof(IClockService), typeof(ClockService));
    services.AddSingleton(typeof(IChatAppConnection), typeof(ChatAppConnection));

    services.AddScoped(typeof(IEntitiesBusinessCommon<,,,,>), typeof(EntitiesBusinessCommon<,,,,>));
    services.AddScoped(typeof(IRepositoryMessage), typeof(RepositoryMessageService));
    #endregion
    #region  Business Basic data
    // services.AddScoped(typeof(IBranchBusiness), typeof(BranchBusiness));
    #endregion
  
    

    #region  Chat 
    services.AddScoped(typeof(IChatBusiness), typeof(ChatBusiness));

    #endregion
    #region  Auth
    services.AddScoped(typeof(IAuthBusiness), typeof(AuthBusiness));
    services.AddScoped(typeof(IRoleBusiness), typeof(RoleBusiness));
    services.AddScoped(typeof(IUserBusiness), typeof(UserBusiness));
    services.AddScoped(typeof(IClaimAppBusiness), typeof(ClaimAppBusiness));
    services.AddScoped(typeof(IRepoClaimApp), typeof(RepoClaimApp));
    services.AddScoped(typeof(IScreenAppBusiness), typeof(ScreenAppBusiness));
    services.AddScoped(typeof(IModuleAppBusiness), typeof(ModuleAppBusiness));
    services.AddScoped(typeof(IAuditAppBusiness), typeof(AuditAppBusiness));
    #endregion
    #region Validator
    // services.AddFluentValidationAutoValidation();
    services.AddTransient<IValidator<ScreenAppRegisterDto>, ScreenAppValidator>();
    services.AddTransient<IValidator<ModuleAppRegisterDto>, ModuleAppValidator>();
    #endregion
    // 
    //  services.AddValidatorsFromAssemblyContaining<HospitalValidator>();   
    #region  Localization
    services.AddScoped(typeof(IRepoLanguageText), typeof(RepoLanguageText));
    services.AddScoped(typeof(ILanguageBusiness), typeof(LanguageBusiness));
    services.AddScoped(typeof(ILanguageKeyBusiness), typeof(LanguageKeyBusiness));
    services.AddScoped(typeof(ILanguageTextBusiness), typeof(LanguageTextBusiness));
    #endregion
    services.AddHttpClient();
    return services;
  }
}

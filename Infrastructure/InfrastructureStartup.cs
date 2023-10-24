 
using Core.Dtos.Settings;
using Core.Interfaces;
using Core.Interfaces.Common;

using Infrastructure.Data;
using Infrastructure.Repository;

using Infrastructure.Services;
using Infrastructure.Services.Common;
using Infrastructure.Services.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
namespace Infrastructure;
public static class InfrastructureStartup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
     Action<DbContextOptionsBuilder> options,
       string ConfigurationToken)
    {
        #region  DbContext
        services.AddDbContext<AppDbContext>(options);
         #endregion
        #region common services
       services.AddScoped(typeof(ICuctomTree), typeof(CuctomTree));

        services.AddHttpContextAccessor();
        services.AddSwaggerServices();
         services.AddScoped(typeof(IRepositoryApp<>), typeof(RepositoryApp<>));
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
         services.AddScoped(typeof(IRepositoryAppHepler<>), typeof(RepositoryAppHepler<>));
        

         #endregion

        #region  log
        services.AddScoped(typeof(ILogCustom), typeof(LogCustom));
        #endregion
        #region  Localization
        services.AddLocalization();
        services.AddDistributedMemoryCache();
        services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
        services.AddSingleton<IStringLocalizerCustom, JsonStringLocalizer>();
        #endregion
        #region  permission & auth
        services.AddAuthenticationServices(ConfigurationToken);
        services.AddScoped(typeof(IRolePermission), typeof(RolePermission));
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        #endregion
        return services;
    }
}

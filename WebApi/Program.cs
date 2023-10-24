using System.Globalization;
using System.Net;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Users.API.ActionFilter;
using Core.Dtos.Settings;
using Infrastructure.Helper.ExtentionMethod;
using Serilog;
using Core.Interfaces.Common;
using Infrastructure.Services.Middlewares;
using Application;
using Core.Exceptions;
using Infrastructure.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
   .ReadFrom.Configuration(builder.Configuration)
   .CreateLogger();
try
{
    builder.Services.AddInfrastructure(
         options => { options.UseSqlServer(builder.Configuration.GetConnectionString("DevelopmentConnection")); },
          builder.Configuration.GetSection("AppSettings:Token").Value);
    builder.Services.AddDbContext<AppHelperContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("HelperConnection")));
    builder.Services.AddApplication();

    builder.Services.AddControllers();
    builder.Services.AddCors();
    builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
    builder.Services.AddScoped(typeof(ValidateRoleExist));
    builder.Services.AddScoped(typeof(ValidateRoleEditExist));
    builder.Services.Configure<MaxTimeToken>(builder.Configuration.GetSection("AppSettings"));
    builder.Services.AddSignalR();
    builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
    var app = builder.Build();
    using var scope = app.Services.CreateScope();
    var logCustom = scope.ServiceProvider.GetRequiredService<ILogCustom>();
    logCustom.AppStartClose("appstart", true);
    if (app.Environment.IsDevelopment())
    {
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
    }
    app.UseExceptionHandler(BuilderExtensions =>
    {
        BuilderExtensions.Run(async context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var error = context.Features.Get<IExceptionHandlerFeature>();
            if (error != null)
            {
                if (error.Error is ExceptionCommonReponse)
                    context.Response.StatusCode = ((ExceptionCommonReponse)error.Error).StatusCode;
                logCustom.GlobalError(error.Error.Message, context.Response.StatusCode);
                context.Response.AddApplicationError(error.Error.Message);
                await context.Response.WriteAsync(error.Error.Message);
            }
        });
    });

    // app.UseMiddleware<ErrorHandlerMiddleware>();
    var cultures = new List<CultureInfo> {
                           new CultureInfo("en"),
                           new CultureInfo("ar") };

    app.UseRequestLocalization(options =>
    {
        options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("ar");
        options.SupportedCultures = cultures;
        options.SupportedUICultures = cultures;
    });
    // app.UseMiddleware<BearerAuthenticationMiddleware>();
    app.UseDefaultFiles();
    app.CustomStaticFiles(builder.Configuration);
    app.UseRouting();
    
    // app.MapHub<ChatHub>("/chatHub");
    app.UseCors(x => x
   .AllowAnyMethod()
   .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
   .AllowCredentials()); // allow credentials
    app.UseAuthentication();
    app.UseAuthorization();
    #pragma warning disable 
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
   


    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Warning(ex, "AppStopUnexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

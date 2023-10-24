using System.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Infrastructure.Services.Middlewares;
public static class StaticFiles
{
    public static IApplicationBuilder CustomStaticFiles(this IApplicationBuilder app, IConfiguration conf)
    {
        app.UseStaticFiles();
       

        //var PhysicalStudyUpoadedPath = Path.Combine(Directory.GetCurrentDirectory(), conf.GetSection("AppSettings:PhysicalStudyUpoadedPath").Value);
        //if (!Directory.Exists(PhysicalStudyUpoadedPath))
        //    Directory.CreateDirectory(PhysicalStudyUpoadedPath);
        //app.UseStaticFiles(
        //    new StaticFileOptions()
        //    {
        //        FileProvider = new PhysicalFileProvider(PhysicalStudyUpoadedPath),
        //        RequestPath = new PathString(conf.GetSection("AppSettings:ServerStudyUpoadedPath").Value)
        //    });
         
        

        return app;
    }
}

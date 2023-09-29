using Microsoft.EntityFrameworkCore;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Infrastructure.Data;

namespace PropertyBuildingDemo.Api
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using PropertyBuildingDemo.Api.Middleware;
    using PropertyBuildingDemo.Application.Extensions;
    using PropertyBuildingDemo.Domain.Entities.Enums;
    using PropertyBuildingDemo.Domain.Entities.Identity;
    using PropertyBuildingDemo.Domain.Interfaces;
    using PropertyBuildingDemo.Infrastructure;
    using PropertyBuildingDemo.Infrastructure.Data;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            Startup.ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();
            Startup.Configure(app);
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var systemLogger = scope.ServiceProvider.GetService<ISystemLogger>();
                try
                {
                    var context = services.GetRequiredService<PropertyBuildingContext>();
                    await context.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    systemLogger.LogExceptionMessage(ELogginLevel.Level_Error, "main startup", ex);
                }
            }
            app.Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
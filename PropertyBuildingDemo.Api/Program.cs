using Microsoft.EntityFrameworkCore;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Infrastructure.Data;

namespace PropertyBuildingDemo.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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
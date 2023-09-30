using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PropertyBuildingDemo.Infrastructure.Data;
using System.Data.Common;
using PropertyBuildingDemo.Api;

namespace PropertyBuildingDemo.Tests
{
    public class ApiWebApplicationFactory
        : WebApplicationFactory<Program>
    {
        private DbConnection _connection;
        private string _dbName;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddLogging(builder => builder.AddConsole().AddDebug());
            });
            builder.ConfigureServices(services =>
            {
                _dbName = Guid.NewGuid().ToString();
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<PropertyBuildingContext>));

                services.Remove(dbContextDescriptor);

                services.AddDbContext<PropertyBuildingContext>((container, options) =>
                {
                    var connectionString = $"Server=(localdb)\\MSSQLLocalDB;Database={_dbName};Integrated Security=True;";
                    options.UseSqlServer(connectionString);
                });
            });

            builder.UseEnvironment("Development");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}

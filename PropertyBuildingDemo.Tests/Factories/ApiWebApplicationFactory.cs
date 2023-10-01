using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PropertyBuildingDemo.Infrastructure.Data;
using System.Data.Common;
using PropertyBuildingDemo.Api;
using Microsoft.Data.Sqlite;

namespace PropertyBuildingDemo.Tests.Factories
{
    public class ApiWebApplicationFactory
        : WebApplicationFactory<Program>
    {
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
            if (disposing)
            {
                //DisposeDatabase();
            }
            base.Dispose(disposing);
        }

        private void DisposeDatabase()
        {
            //string databaseFilePath = $"(localdb)\\{_dbName}";

            //// Depending on the database provider, you may need to adjust the file path extraction
            //if (!string.IsNullOrEmpty(databaseFilePath))
            //{
            //    // Extract the database file path from the connection string
            //    // and delete the file
            //    try
            //    {
            //        System.IO.File.Delete(databaseFilePath);
            //    }
            //    catch (Exception ex)
            //    {
            //        // Handle any exceptions that may occur during file deletion
            //        // Log the exception or take appropriate action
            //    }
            //}
        }
    }
}

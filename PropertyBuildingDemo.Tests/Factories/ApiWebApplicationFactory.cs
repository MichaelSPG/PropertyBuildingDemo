using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PropertyBuildingDemo.Api;
using PropertyBuildingDemo.Infrastructure.Data;

namespace PropertyBuildingDemo.Tests.Factories
{
    /// <summary>
    /// A custom WebApplicationFactory used for integration testing an ASP.NET Core application.
    /// </summary>
    public class ApiWebApplicationFactory : WebApplicationFactory<Program>
    {
        private string _dbName;

        /// <summary>
        /// Configures the WebHost for integration testing.
        /// </summary>
        /// <param name="builder">The WebHost builder to configure.</param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Configure test services.
            builder.ConfigureTestServices(services =>
            {
                services.AddLogging(builder => builder.AddConsole().AddDebug());
            });

            // Configure services for the test context.
            builder.ConfigureServices(services =>
            {
                // Generate a unique database name.
                _dbName = Guid.NewGuid().ToString();

                // Remove the existing DbContextOptions<PropertyBuildingContext>.
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<PropertyBuildingContext>));
                services.Remove(dbContextDescriptor);

                // Add a new DbContext with the generated database name and a localdb connection string.
                services.AddDbContext<PropertyBuildingContext>((container, options) =>
                {
                    var connectionString = $"Server=(localdb)\\MSSQLLocalDB;Database={_dbName};Integrated Security=True;";
                    options.UseSqlServer(connectionString);
                });
            });

            // Set the environment to "Development" for the test application.
            builder.UseEnvironment("Development");
        }

        /// <summary>
        /// Dispose method to clean up resources, including the database.
        /// </summary>
        /// <param name="disposing">A boolean indicating whether to dispose of resources.</param>
        private void DisposeDatabase()
        {
            string databaseFilePath = $"(localdb)\\MSSQLLocalDB";

            // Depending on the database provider, you may need to adjust the file path extraction
            if (!string.IsNullOrEmpty(databaseFilePath))
            {
                // Extract the database file path from the connection string
                // and delete the file
                try
                {
                    System.IO.File.Delete(databaseFilePath);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that may occur during file deletion
                    // Log the exception or take appropriate action
                }
            }
        }
    }
}

// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using PropertyBuildingDemo.Infrastructure.Data.Seed;

namespace PropertyBuildingDemo.Api;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Infrastructure.Data;

/// <summary>
/// The entry point class for the application.
/// </summary>
public class Program
{
    /// <summary>
    /// The entry point of the application.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public static async Task Main(string[] args)
    {
        // Create a web application builder
        var builder = WebApplication.CreateBuilder(args);

        // Configure services and dependencies using Startup class
        Startup.ConfigureServices(builder.Services, builder.Configuration);

        // Build the application
        var app = builder.Build();

        // Configure the application
        Startup.Configure(app);

        // Create a scope to manage service lifetimes
        using (var scope = app.Services.CreateScope())
        {
            // Retrieve services
            var services = scope.ServiceProvider;
            var systemLogger = scope.ServiceProvider.GetService<ISystemLogger>();

            try
            {
                // Get the database context and perform migrations
                var context = services.GetRequiredService<PropertyBuildingContext>();
                await context.Database.MigrateAsync();

                // Seed user data using UserManager
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await SeedIdentityDbContext.SeedUserData(userManager);
                await SeedPropertyDbContext.SeedPropertyBuildingData(context);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during startup
                systemLogger.LogExceptionMessage(ELoggingLevel.Error, "Main Startup", ex);
            }
        }

        // Start the application
        await app.RunAsync();
    }
}
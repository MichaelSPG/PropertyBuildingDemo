﻿// <copyright file="Startup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PropertyBuildingDemo.Api;

using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using PropertyBuildingDemo.Api.Extensions;
using PropertyBuildingDemo.Api.Middleware;
using PropertyBuildingDemo.Infrastructure;

/// <summary>
/// The startup class for configuring and initializing the application.
/// </summary>
public class Startup
{
    /// <summary>
    /// Configures the services and dependencies for the application.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application configuration.</param>
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Register infrastructure services and dependencies
        services.RegisterIntrastrucureServices(configuration);

        // Add application-specific services
        services.AddApplicationServices();

        // Add controllers
        services.AddControllers();

        // Configure Swagger/OpenAPI for API documentation
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSwaggerDocumentation();

        // Configure API versioning
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });

        // Configure CORS policy for API controllers
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("*"));
        });

        // Add localization support
        services.AddLocalization();
    }

    /// <summary>
    /// Configures the application's request processing pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    public static void Configure(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            // Enable Swagger UI for development
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Enable HTTPS redirection
        app.UseHttpsRedirection();

        // Configure CORS policy
        app.UseCors("CorsPolicy");

        // Use custom exception handling middleware
        app.UseMiddleware<ExceptionMiddleware>();

        // Enable HTTPS redirection
        app.UseHttpsRedirection();

        // Enable authentication and authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Map controllers
        app.MapControllers();

        // Configure request localization to use "en-US" culture
        app.UseRequestLocalization("en-US");
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.DefaultThreadCurrentCulture;
    }
}
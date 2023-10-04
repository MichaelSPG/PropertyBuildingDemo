// <copyright file="SwaggerExtensions.cs" company="@Michael.PatinoDemos">
// Copyright (c) 
// </copyright>

namespace PropertyBuildingDemo.Api.Extensions;

using Microsoft.OpenApi.Models;

/// <summary>
/// Extension methods for configuring Swagger documentation in an ASP.NET Core application.
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Adds Swagger documentation generation and configuration to the provided <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // Configure Swagger document information
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "PropertyBuildingDemo.Api",
                Version = "v1",
            });

            // Define security scheme for JWT Bearer token
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "access_token",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"[access_token]=2sfa312342342dsda\"",
            });

            // Define security requirement for Bearer token
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                    },
                    new string[] { }
                },
            });
        });

        return services;
    }
}
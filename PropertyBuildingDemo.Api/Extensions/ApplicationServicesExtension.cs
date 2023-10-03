// <copyright file="ApplicationServicesExtension.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PropertyBuildingDemo.Api.Extensions;

using Microsoft.Extensions.DependencyInjection.Extensions;
using PropertyBuildingDemo.Application.IServices;
using PropertyBuildingDemo.Application.Mappings;
using PropertyBuildingDemo.Application.Services;
using PropertyBuildingDemo.Domain.Interfaces;

/// <summary>
/// Extension methods for configuring application services in an ASP.NET Core application.
/// </summary>
public static class ApplicationServicesExtension
{
    /// <summary>
    /// Adds application-specific services to the provided <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register HttpContextAccessor as a singleton service
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // Register AutoMapper with profiles from the specified assembly (EntityAndDtoMapping)
        services.AddAutoMapper(typeof(EntityAndDtoMapping));

        // Register application services with their respective lifetimes
        services.AddScoped<IPropertyBuildingService, PropertyService>();
        services.AddScoped<IApiTokenService, TokenService>();
        services.AddScoped<IUserAccountService, UserAccountService>();
        services.AddScoped(typeof(IDbEntityServices<,>), typeof(DbEntityServices<,>));

        // Register CurrentUserService as a transient service
        services.AddTransient<ICurrentUserService, CurentUserService>();

        // Register HttpContextAccessor again for the CurrentUserService
        services.AddHttpContextAccessor();

        return services;
    }
}
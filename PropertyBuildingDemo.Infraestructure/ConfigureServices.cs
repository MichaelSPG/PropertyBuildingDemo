﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PropertyBuildingDemo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Infrastructure.Repositories;
using PropertyBuildingDemo.Infrastructure.Logging;
using log4net.Config;

namespace PropertyBuildingDemo.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration )
        {
            services.AddDbContext<PropertyBuildingContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericEntityRepository<>), typeof(BaseEntityRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            return services;
        }
        public static IServiceCollection AddInfrastuctureBase(this IServiceCollection services)
        {
            services.AddSingleton<ISystemLogger, DefaultSystemLogger>();
            return services;
        }

        public static IServiceCollection RegisterIntrastrucureServices(this IServiceCollection services, IConfiguration configuration)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));

            AddRepositories(services);
            AddDatabaseContext(services, configuration);
            AddInfrastuctureBase(services);
            return services;
        }
        
    }
}
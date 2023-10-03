using System.Configuration;
using System.Net;
using System.Security.Claims;
using log4net.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Infrastructure.Data;
using PropertyBuildingDemo.Infrastructure.Logging;
using PropertyBuildingDemo.Infrastructure.Repositories;
using PropertyBuildingDemo.Infrastructure.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PropertyBuildingDemo.Domain.Common;
using System.Globalization;
using PropertyBuildingDemo.Application.Config;
using StackExchange.Redis;

namespace PropertyBuildingDemo.Infrastructure
{
    /// <summary>
    /// A static class for configuring services in the application.
    /// </summary>
    public static class ConfigureServices
    {
        /// <summary>
        /// Adds the database context to the services collection.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<PropertyBuildingContext>(options =>
            {
                options.UseSqlServer(conn, sqlServerOptionsAction:
                    sqlOptions =>
                    {
                        options.EnableSensitiveDataLogging();
                        options.EnableDetailedErrors();
                        sqlOptions.CommandTimeout(120);
                        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromMilliseconds(500), null);
                    });
            });

            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var config = ConfigurationOptions.
                    Parse(configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(config);
            });
            return services;
        }

        /// <summary>
        /// Adds repositories to the services collection.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericEntityRepository<>), typeof(BaseEntityRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        /// <summary>
        /// Adds infrastructure services to the services collection.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddInfrastuctureBase(this IServiceCollection services)
        {
            services.AddSingleton<ISystemLogger, DefaultSystemLogger>();
            return services;
        }

        /// <summary>
        /// Adds identity services to the services collection.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultIdentity<AppUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<PropertyBuildingContext>()
            .AddClaimsPrincipalFactory<UserClaimsFactory>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });

            return services;
        }

        /// <summary>
        /// Gets the application settings from the configuration.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The application settings.</returns>
        public static ApplicationConfig GetApplicationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationSettingsConfiguration = configuration.GetSection(nameof(ApplicationConfig));
            services.Configure<ApplicationConfig>(applicationSettingsConfiguration);
            return applicationSettingsConfiguration.Get<ApplicationConfig>();
        }

        /// <summary>
        /// Adds JWT authentication to the services collection.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = Encoding.UTF8.GetBytes(services.GetApplicationSettings(configuration).Secret);
            services.AddAuthentication(authentication =>
            {
            })
           .AddJwtBearer("JwtClient", options =>
           {
               options.RequireHttpsMetadata = false;
               options.SaveToken = true;
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   ValidateLifetime = true,
                   RoleClaimType = ClaimTypes.Role,
                   ClockSkew = TimeSpan.Zero
               };
               options.Events = new JwtBearerEvents
               {
                   OnMessageReceived = context =>
                   {
                       var accessToken = context.Request.Headers["access_token"];

                       // If the request is for our hub...
                       var path = context.HttpContext.Request.Path;
                       if (!string.IsNullOrEmpty(accessToken))
                       {
                           // Read the token out of the query string
                           context.Token = accessToken;
                       }
                       return Task.CompletedTask;
                   },
                   OnAuthenticationFailed = c =>
                   {
                       if (c.Exception is SecurityTokenExpiredException)
                       {
                           c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                           c.Response.ContentType = "application/json";
                           var result = JsonConvert.SerializeObject(ApiResult.FailedResult(c.Response.StatusCode, "The Token is expired."));
                           return c.Response.WriteAsync(result);
                       }
                       else
                       {
#if DEBUG
                           c.NoResult();
                           c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                           c.Response.ContentType = "text/plain";
                           var result = JsonConvert.SerializeObject(ApiResult.FailedResult(c.Response.StatusCode, c.Exception.ToString()));
                           return c.Response.WriteAsync(result);
#else
                            c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            c.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(Result.Fail("An unhandled error has occurred."));
                            return c.Response.WriteAsync(result);#endif
#endif
                       }
                   },
                   OnChallenge = context =>
                   {
                       context.HandleResponse();
                       if (!context.Response.HasStarted)
                       {
                           context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                           context.Response.ContentType = "application/json";
                           var result = JsonConvert.SerializeObject(ApiResult.FailedResult(context.Response.StatusCode, "You are not Authorized."));
                           return context.Response.WriteAsync(result);
                       }

                       return Task.CompletedTask;
                   },
                   OnForbidden = context =>
                   {
                       context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                       context.Response.ContentType = "application/json";
                       var result = JsonConvert.SerializeObject(ApiResult.FailedResult(context.Response.StatusCode, "You are not authorized to access this resource."));
                       return context.Response.WriteAsync(result);
                   },
               };
           });
            return services;
        }

        /// <summary>
        /// Adds an HTTP client to the services collection.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            var apiUrl = services.GetApplicationSettings(configuration).BaseUrl;
            services.AddScoped<HttpClient>(s => {
                var client = new HttpClient { BaseAddress = new Uri(apiUrl) };
                client.DefaultRequestHeaders.AcceptLanguage.Clear();
                client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
                return client;
            });
            return services;
        }

        /// <summary>
        /// Registers infrastructure services in the services collection.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection RegisterIntrastrucureServices(this IServiceCollection services, IConfiguration configuration)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));

            AddRepositories(services);
            AddDatabaseContext(services, configuration);
            AddIdentityServices(services, configuration);
            AddInfrastuctureBase(services);
            AddJwtAuthentication(services, configuration);
            AddHttpClient(services, configuration);
            return services;
        }
    }
}

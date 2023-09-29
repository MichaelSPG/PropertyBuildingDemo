using Microsoft.Extensions.DependencyInjection;
using PropertyBuildingDemo.Application.IServices;
using PropertyBuildingDemo.Application.Mappings;
using PropertyBuildingDemo.Application.PropertyServices;
using PropertyBuildingDemo.Application.Services;
using PropertyBuildingDemo.Domain.Interfaces;

namespace PropertyBuildingDemo.Application.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper(typeof(EntityAndDtoMapping));
            services.AddScoped<IPropertyBuildingService, PropertyService>();
            services.AddScoped<IPropertyImageService, PropertyImageService>();
            services.AddScoped<IPropertyTraceService, PropertyTraceService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserAccountService, UsertAccountService>();
           
            //services.AddTransient<ICurrentUserService, CurentUserService>();
            //services.AddHttpContextAccessor();
            return services;
        }
    }
}

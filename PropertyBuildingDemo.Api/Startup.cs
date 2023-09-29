﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using PropertyBuildingDemo.Api.Middleware;
using PropertyBuildingDemo.Application.Extensions;
using PropertyBuildingDemo.Infrastructure;

namespace PropertyBuildingDemo.Api
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration Configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterIntrastrucureServices(Configuration);
            services.AddApplicationServices();
            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });
        }

        public static  void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}

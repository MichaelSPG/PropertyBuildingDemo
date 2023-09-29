using Microsoft.AspNetCore.Http;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Interfaces;
using System.Net;
using System.Text.Json;
using PropertyBuildingDemo.Domain.Entities.Enums;

namespace PropertyBuildingDemo.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ISystemLogger          _systemLogger;
        private readonly RequestDelegate        _next;
        private readonly IHostEnvironment       _hostEnvironment;
        public ExceptionMiddleware(ISystemLogger InSystemLogger, RequestDelegate next, IHostEnvironment hostEnvironment)
        {
            _systemLogger = InSystemLogger;
            _next = next;
            _hostEnvironment = hostEnvironment;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {                
                _systemLogger.LogExceptionMessage(ELogginLevel.Level_Error, ex.Message, ex);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ApiResult apiResult = ApiResult.FailedResult((int)HttpStatusCode.InternalServerError, ex.Message);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(apiResult, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}

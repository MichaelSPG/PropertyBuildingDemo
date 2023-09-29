using Microsoft.AspNetCore.Http;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Interfaces;
using System.Net;
using System.Text.Json;

namespace PropertyBuildingDemo.Api.Middleware
{
    public class AuthorizarionMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizarionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var tokenValidationService = context.RequestServices.GetRequiredService<ITokenService>();

            // If the token is invalid, throw an exception or set a response status code.

            string accessToken = context.Request.Headers["access_token"];

            // Tipically handled by default auhtorization, so is a unauthorized result

            ApiResult<string> result = await tokenValidationService.ValidateToken(accessToken);
            if (!string.IsNullOrWhiteSpace(accessToken) && !result.IsSuccess())
            {
                context.Response.StatusCode = 401;
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(result, options);
                await context.Response.WriteAsync(json);
                return;
            }
            await _next(context);
        }
    }
}

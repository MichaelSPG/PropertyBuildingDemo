// <copyright file="ExceptionMiddleware.cs" company="@Michael.PatinoDemos">
// Copyright (c) 
// </copyright>

namespace PropertyBuildingDemo.Api.Middleware;

using System.Net;
using System.Text.Json;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Interfaces;

/// <summary>
/// Middleware for handling exceptions and providing standardized error responses to clients.
/// </summary>
public class ExceptionMiddleware
{
    private readonly ISystemLogger _systemLogger; // Dependency for logging exceptions
    private readonly RequestDelegate _next; // The next middleware in the pipeline

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
    /// </summary>
    /// <param name="inSystemLogger">The system logger for recording exceptions.</param>
    /// <param name="next">The next middleware in the request pipeline.</param>
    public ExceptionMiddleware(ISystemLogger inSystemLogger, RequestDelegate next)
    {
        this._systemLogger = inSystemLogger;
        this._next = next;
    }

    /// <summary>
    /// Invokes the middleware to handle exceptions during request processing.
    /// </summary>
    /// <param name="context">The HTTP context representing the current request and response.</param>
    /// <returns>the task</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Continue processing the request by invoking the next middleware
            await this._next(context);
        }
        catch (Exception ex)
        {
            // Log the caught exception using the provided system logger
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, ex.Message, ex);

            // Set response properties for error handling
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Create an API result object to encapsulate the error information
            ApiResult apiResult = ApiResult.FailedResult((int)HttpStatusCode.InternalServerError, ex.Message);

            // Configure JSON serialization options to use camel case naming for properties
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            // Serialize the API result to JSON
            var json = JsonSerializer.Serialize(apiResult, options);

            // Write the JSON response to the client
            await context.Response.WriteAsync(json);
        }
    }
}
namespace PropertyBuildingDemo.Api.Filters
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using PropertyBuildingDemo.Domain.Common;

    /// <summary>
    /// Action filter for validating model DTOs in ASP.NET Core controllers.
    /// </summary>
    public class DtoModelValidationFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Overrides the method to execute custom logic before an action method is executed.
        /// </summary>
        /// <param name="context">The action executing context.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Check if the model state is not valid
            if (!context.ModelState.IsValid)
            {
                // Extract validation error messages
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                // Create a custom response with the validation errors
                var response = ApiResult.FailedResult(errors);

                // Set the result to a BadRequestObjectResult with the custom response
                context.Result = new BadRequestObjectResult(response);
            }
        }
    }
}

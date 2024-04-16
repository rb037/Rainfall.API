using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Rainfall.API.Data.DTO;

namespace Rainfall.API.Filters
{
    /// <summary>
    /// Action filter to validate the model state before the action is executed.
    /// </summary>
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="context">The action executing context.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ValidateModelStateAttribute>>();
                logger.LogWarning("Model validation failed for action {Action}", context.ActionDescriptor.DisplayName);

                var errorResponse = new ErrorResponse
                {
                    Message = "Invalid request",
                    Detail = context.ModelState
                        .SelectMany(m => m.Value!.Errors
                            .Select(e => new ErrorDetail { PropertyName = m.Key, Message = e.ErrorMessage }))
                        .ToList()
                };
                context.Result = new BadRequestObjectResult(errorResponse);
            }
        }
    }

}

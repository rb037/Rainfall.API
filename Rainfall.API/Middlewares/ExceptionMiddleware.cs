using Newtonsoft.Json;
using Rainfall.API.Data.DTO;
using System.Net;

namespace Rainfall.API.Middlewares
{
    /// <summary>
    /// Middleware to handle exceptions globally across the application.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">The logger.</param>
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Handles the exception by writing an error response.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)GetStatusCode(exception);
            var response = new ErrorResponse { Message = exception.Message };
            var result = JsonConvert.SerializeObject(response);
            return context.Response.WriteAsync(result);
        }

        /// <summary>
        /// GetStatusCode (Default is 500)
        /// </summary>
        /// <param name="exception">The exception</param>
        /// <returns>HttpStatusCode</returns>
        private static HttpStatusCode GetStatusCode(Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            // Determine the status code based on the exception type
            if (exception is JsonException)
            {
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (exception is HttpRequestException)
            {
                statusCode = HttpStatusCode.ServiceUnavailable;
            }

            return statusCode;
        }
    }
}


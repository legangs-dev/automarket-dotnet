using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AutoMarket.API.Applications.Exceptions
{
    public class GlobalExceptionHandler() : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails();
            problemDetails.Instance = httpContext.Request.Path;

            switch (exception)
            {
                case ValidationException validationException:
                    problemDetails.Title = "Validation error";
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    problemDetails.Detail = "One or more validation errors occurred.";

                    problemDetails.Extensions["errors"] = validationException.Errors;
                    break;
                default:
                    problemDetails.Title = "An error occurred";
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Detail = exception.Message;
                    break;
            }

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
            return true;
        }
    }
};

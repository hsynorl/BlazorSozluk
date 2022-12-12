using BlazorSozluk.Common.Infrastructure.Exceptions;
using BlazorSozluk.Common.Infrastructure.Results;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace BlazorSozluk.Api.WebApi.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder CongigureExceptionHandling(
            this IApplicationBuilder app,
            bool includeExcepitonDetails = false,
            bool useDefaultHandlingResponse = true,
            Func<HttpContext, Exception, Task> handleExcepiton = null
            )
        {
            app.Run(context =>
            {
                var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();
                if (!useDefaultHandlingResponse && handleExcepiton == null)
                {
                    throw new ArgumentNullException(nameof(handleExcepiton),
                        $"{nameof(handleExcepiton)}  cannot be null when {nameof(useDefaultHandlingResponse)} is false");

                }
                if (!useDefaultHandlingResponse && handleExcepiton != null)
                {
                    return handleExcepiton(context, exceptionObject.Error);
                }
                return DefaultHandleException(context, exceptionObject.Error, includeExcepitonDetails);
            });
            return app;

        }


        private static async Task DefaultHandleException(HttpContext context, Exception exception, bool includeExcepitonDetails)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "Internal sever error occured!";
            if (exception is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            if (exception is DatabaseValidationExcepiton)
            {
                var validationResponse = new ValidationResponseModel(exception.Message);
                await WriteResponse(context, statusCode, validationResponse);
                return;
            }
            var res = new
            {
                HttpStatusCode = (int)statusCode,
                Detail = includeExcepitonDetails ? exception.ToString() : message
            };
            await WriteResponse(context, statusCode, res);

        }

        private static async Task WriteResponse(HttpContext context, HttpStatusCode statusCode, object responseObject)
        {

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsJsonAsync(responseObject);
        }

    }
}

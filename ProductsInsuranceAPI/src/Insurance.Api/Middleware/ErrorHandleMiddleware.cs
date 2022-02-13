using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Insurance.Utilities.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProblemDetails = Insurance.Utilities.ErrorHandling.ProblemDetails;

namespace Insurance.Api.Middleware
{
    public class ErrorHandleMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandleMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext httpContext, ILogger<ErrorHandleMiddleware> logger)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ClientException e)
            {
                await HandleException(httpContext, HttpStatusCode.BadRequest, e, logger);
            }
            catch (Exception e)
            {
                var tracingId = Guid.NewGuid().ToString();
                logger.LogError(e, "tracingId={0},Error Message:{1}", tracingId,e.Message);
                await HandleException(httpContext, new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Something went wrong",
                    TraceId = tracingId
                });
            }
        }

        private static Task HandleException(HttpContext context, HttpStatusCode statusCode, Exception e, ILogger<ErrorHandleMiddleware> logger)
            =>  HandleException(context, statusCode, e, statusCode.ToString(), logger);

        private static Task HandleException(HttpContext context, HttpStatusCode statusCode, Exception e, string title,ILogger<ErrorHandleMiddleware> logger)
        {
            var tracingId = Guid.NewGuid().ToString();
            logger.LogInformation(e, "tracingId={0}, Message:{1}", tracingId, e.Message);

            var details = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = title,
                TraceId = tracingId,
                Details = e.Message
            };
            return HandleException(context, details);
        }

        private static Task HandleException(HttpContext context, ProblemDetails details)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)details.Status;

            var res = Serialize(details);
            return context.Response.WriteAsync(res);
        }

        private static string Serialize(object example) => JsonSerializer.Serialize(example, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IgnoreNullValues = true,
            Converters = { new JsonStringEnumConverter() }
        });
    }
}

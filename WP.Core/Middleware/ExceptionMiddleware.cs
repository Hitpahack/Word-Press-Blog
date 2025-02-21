using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WP.Core.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
            string requestPath = context.Request.Path;
            string requestMethod = context.Request.Method;
            string errorId = Guid.NewGuid().ToString(); // Unique error ID for tracking

            // Set the default status code to 500 (Internal Server Error)
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var errorType = "Server Error";

            // Categorize exceptions based on type
            if (exception is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                errorType = "Unauthorized Access";
            }
            else if (exception is ArgumentException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                errorType = "Bad Request";
            }
            else if (exception is KeyNotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                errorType = "Resource Not Found";
            }

            // Log the error with relevant details
            _logger.LogError(exception, 
                "Error ID: {ErrorId} | IP: {ClientIp} | Path: {RequestPath} | Method: {RequestMethod} | Status: {StatusCode} | Message: {Message}",
                errorId, clientIp, requestPath, requestMethod, statusCode, exception.Message);

            // Prepare a user-friendly error response
            var errorResponse = new ErrorResponse
            {
                ErrorId = errorId,
                StatusCode = statusCode,
                ErrorType = errorType,
                Message = exception.Message,
                Details = exception.InnerException?.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            // Serialize response and send
            var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions { WriteIndented = true });
            await context.Response.WriteAsync(jsonResponse);
        }
    }

    public class ErrorResponse
    {
        public string ErrorId { get; set; } // Unique ID for tracking errors
        public int StatusCode { get; set; }
        public string ErrorType { get; set; }
        public string Message { get; set; }
        public string? Details { get; set; }
    }
}

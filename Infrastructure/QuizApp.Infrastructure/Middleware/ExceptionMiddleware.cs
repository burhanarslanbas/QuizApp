using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using QuizApp.Application.DTOs.Responses;
using QuizApp.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace QuizApp.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
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
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                Message = exception.Message,
                StatusCode = exception switch
                {
                    BusinessException => StatusCodes.Status400BadRequest,
                    ValidationException => StatusCodes.Status400BadRequest,
                    NotFoundException => StatusCodes.Status404NotFound,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError
                }
            };

            // Development ortamında detaylı hata bilgisi
            if (_env.IsDevelopment())
            {
                errorResponse.Details = exception.StackTrace;
            }

            // Log the exception
            _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

            response.StatusCode = errorResponse.StatusCode;
            var json = JsonSerializer.Serialize(errorResponse);
            await response.WriteAsync(json);
        }
    }
} 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuizApp.Application.Exceptions;
using QuizApp.Application.Exceptions.HttpProblemDetails;
using System.Net;
using System.Text.Json;

namespace QuizApp.Infrastructure.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        ProblemDetails problemDetails = exception switch
        {
            ValidationException validationException => new QuizApp.Application.Exceptions.HttpProblemDetails.ValidationProblemDetails(validationException.Errors),
            NotFoundException notFoundException => new NotFoundProblemDetails(notFoundException.Message),
            ForbiddenAccessException forbiddenException => new ForbiddenProblemDetails(forbiddenException.Message),
            BusinessException businessException => new BusinessProblemDetails(businessException.Message),
            _ => new InternalServerErrorProblemDetails(exception.Message)
        };

        context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(problemDetails, options);
        await context.Response.WriteAsync(json);
    }
}
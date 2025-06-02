using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using QuizApp.Application.Exceptions.Extensions;
using QuizApp.Application.Exceptions.HttpProblemDetails;
using QuizApp.Application.Exceptions.Types;
using System.Net;
using System.Text.Json;

namespace QuizApp.Application.Exceptions.Handlers;

public class HttpExceptionHandler : ExceptionHandler
{
    private HttpResponse? _response;
    private readonly ILogger<HttpExceptionHandler> _logger;

    public HttpExceptionHandler(ILogger<HttpExceptionHandler> logger)
    {
        _logger = logger;
    }

    public HttpResponse Response
    {
        get => _response ?? throw new ArgumentNullException(nameof(_response));
        set => _response = value;
    }

    protected override Task HandleException(BusinessException businessException)
    {
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        string details = new BusinessProblemDetails(businessException.Message).AsJson();
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(FluentValidation.ValidationException validationException)
    {
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        var errors = validationException.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray()
            );

        var details = new Microsoft.AspNetCore.Mvc.ValidationProblemDetails(errors).AsJson();
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(NotFoundException notFoundException)
    {
        Response.StatusCode = StatusCodes.Status404NotFound;
        string details = new NotFoundProblemDetails(notFoundException.Message).AsJson();
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(UnauthorizedAccessException unauthorizedAccessException)
    {
        Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        string details = new UnauthorizedProblemDetails(unauthorizedAccessException.Message).AsJson();
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(ForbiddenAccessException forbiddenAccessException)
    {
        Response.StatusCode = (int)HttpStatusCode.Forbidden;
        string details = new ForbiddenProblemDetails(forbiddenAccessException.Message).AsJson();
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(Exception exception)
    {
        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        _logger.LogError(exception, "An unexpected error occurred");
        string details = new InternalServerErrorProblemDetails(exception.Message).AsJson();
        return Response.WriteAsync(details);
    }
} 
using Microsoft.AspNetCore.Http;

namespace QuizApp.Application.Exceptions.HttpProblemDetails;

public class ValidationProblemDetails : Microsoft.AspNetCore.Mvc.ValidationProblemDetails
{
    public ValidationProblemDetails(IDictionary<string, string[]> errors) : base(errors)
    {
        Title = "Validation Error";
        Status = StatusCodes.Status400BadRequest;
        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
    }
}
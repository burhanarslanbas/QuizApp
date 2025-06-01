using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuizApp.Application.Exceptions.HttpProblemDetails;

public class ForbiddenProblemDetails : ProblemDetails
{
    public ForbiddenProblemDetails(string detail)
    {
        Title = "Forbidden";
        Detail = detail;
        Status = StatusCodes.Status403Forbidden;
        Type = "https://quizapp.com/props/forbidden";
    }
} 
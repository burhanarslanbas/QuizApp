using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuizApp.Application.Exceptions.HttpProblemDetails;

public class UnauthorizedProblemDetails : ProblemDetails
{
    public UnauthorizedProblemDetails(string detail)
    {
        Title = "Unauthorized";
        Detail = detail;
        Status = StatusCodes.Status401Unauthorized;
        Type = "https://quizapp.com/props/unauthorized";
    }
}
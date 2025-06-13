using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuizApp.Application.Exceptions.HttpProblemDetails;

public class BusinessProblemDetails : ProblemDetails
{
    public BusinessProblemDetails(string detail)
    {
        Title = "Business Rule Violation";
        Detail = detail;
        Status = StatusCodes.Status400BadRequest;
        Type = "https://quizapp.com/props/business";
    }
}
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace QuizApp.Application.Exceptions.Extensions;

public static class ProblemDetailsExtensions
{
    public static string AsJson(this ProblemDetails details)
    {
        return JsonSerializer.Serialize(details);
    }
}
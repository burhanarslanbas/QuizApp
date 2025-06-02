using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace QuizApp.Application.Exceptions.Extensions;

public static class ProblemDetailsExtensions
{
    public static string AsJson(this ProblemDetails details)
    {
        return JsonSerializer.Serialize(details);
    }
} 
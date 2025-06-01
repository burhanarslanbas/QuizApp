namespace QuizApp.Application.DTOs.Responses.Auth;

public class LoginErrorResponse
{
    public bool Success { get; set; }
    public List<string> Errors { get; set; } = new();
} 
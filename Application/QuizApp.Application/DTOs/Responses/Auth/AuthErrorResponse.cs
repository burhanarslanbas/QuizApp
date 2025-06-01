namespace QuizApp.Application.DTOs.Responses.Auth;

public class AuthErrorResponse
{
    public bool Success { get; set; }
    public required List<string> Errors { get; set; }
} 
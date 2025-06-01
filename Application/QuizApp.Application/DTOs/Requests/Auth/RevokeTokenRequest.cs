namespace QuizApp.Application.DTOs.Requests.Auth;

public class RevokeTokenRequest
{
    public string RefreshToken { get; set; } = null!;
} 
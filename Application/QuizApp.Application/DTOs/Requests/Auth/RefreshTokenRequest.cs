namespace QuizApp.Application.DTOs.Requests.Auth;

public class RefreshTokenRequest
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
} 
namespace QuizApp.Application.DTOs.Requests.Auth;

public class RevokeTokenRequest
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
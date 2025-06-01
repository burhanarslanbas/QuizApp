namespace QuizApp.Application.DTOs.Requests.Auth;

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public bool RememberMe { get; set; }
} 
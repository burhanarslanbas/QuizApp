namespace QuizApp.Application.DTOs.Responses.Auth;

public class LoginSuccessResponse
{
    public bool Success { get; set; }
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
    public required string UserId { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required List<string> Roles { get; set; }
} 
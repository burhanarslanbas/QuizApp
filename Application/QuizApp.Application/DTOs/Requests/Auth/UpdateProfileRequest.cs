namespace QuizApp.Application.DTOs.Requests.Auth;

public class UpdateProfileRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
} 
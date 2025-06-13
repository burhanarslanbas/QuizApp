namespace QuizApp.Application.DTOs.Requests.User;

public record UpdateUserRequest
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
}
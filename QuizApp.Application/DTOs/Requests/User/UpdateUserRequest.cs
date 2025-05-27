using QuizApp.Domain.Enums;

namespace QuizApp.Application.DTOs.Requests.User;

public record UpdateUserRequest
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; } = true;
}
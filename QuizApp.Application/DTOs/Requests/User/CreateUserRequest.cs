using QuizApp.Domain.Enums;

namespace QuizApp.Application.DTOs.Requests.User;

public record CreateUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public UserRole Role { get; set; }
}
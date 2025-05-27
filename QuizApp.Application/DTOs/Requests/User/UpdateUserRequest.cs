using QuizApp.Domain.Enums;

namespace QuizApp.Application.DTOs.Requests.User;

public record UpdateUserRequest
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
}
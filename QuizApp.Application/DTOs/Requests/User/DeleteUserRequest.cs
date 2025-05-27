namespace QuizApp.Application.DTOs.Requests.User;

public record DeleteUserRequest
{
    public Guid Id { get; set; }
}
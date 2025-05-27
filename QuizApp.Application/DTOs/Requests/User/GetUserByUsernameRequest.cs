namespace QuizApp.Application.DTOs.Requests.User;

public record GetUserByUsernameRequest
{
    public string UserName { get; set; }
} 
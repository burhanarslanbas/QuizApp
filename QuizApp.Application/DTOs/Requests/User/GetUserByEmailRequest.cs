namespace QuizApp.Application.DTOs.Requests.User;
 
public record GetUserByEmailRequest
{
    public string Email { get; set; }
} 
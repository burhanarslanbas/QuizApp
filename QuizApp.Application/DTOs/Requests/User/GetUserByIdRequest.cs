namespace QuizApp.Application.DTOs.Requests.User;
 
public record GetUserByIdRequest
{
    public Guid Id { get; set; }
} 
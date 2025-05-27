namespace QuizApp.Application.DTOs.Requests.QuizResult;
 
public record GetQuizResultByIdRequest
{
    public Guid Id { get; set; }
} 
namespace QuizApp.Application.DTOs.Requests.QuizResult;

public record CreateQuizResultRequest
{
    public Guid QuizId { get; set; }
    public Guid StudentId { get; set; }
} 
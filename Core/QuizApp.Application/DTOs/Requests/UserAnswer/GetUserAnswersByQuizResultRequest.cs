namespace QuizApp.Application.DTOs.Requests.UserAnswer;

public record GetUserAnswersByQuizResultRequest
{
    public Guid QuizResultId { get; set; }
} 
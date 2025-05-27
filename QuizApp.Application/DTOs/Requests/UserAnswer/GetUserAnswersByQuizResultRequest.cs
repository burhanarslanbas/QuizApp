namespace QuizApp.Application.DTOs.Requests.UserAnswer;

public record GetUserAnswersByQuizResultRequest
{
    public Guid QuizResultId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 
namespace QuizApp.Application.DTOs.Requests.QuizResult;

public record GetQuizResultsByQuizRequest
{
    public Guid QuizId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 
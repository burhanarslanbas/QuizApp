namespace QuizApp.Application.DTOs.Requests.QuizResult;

public record GetQuizResultsByUserRequest
{
    public Guid UserId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 
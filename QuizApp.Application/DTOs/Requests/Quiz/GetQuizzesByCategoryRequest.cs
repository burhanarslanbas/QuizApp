namespace QuizApp.Application.DTOs.Requests.Quiz;

public record GetQuizzesByCategoryRequest
{
    public Guid CategoryId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 
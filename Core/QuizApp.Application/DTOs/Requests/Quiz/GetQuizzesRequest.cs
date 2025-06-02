namespace QuizApp.Application.DTOs.Requests.Quiz;

public record GetQuizzesRequest
{
    public Guid? CategoryId { get; set; }
    public bool? IsActive { get; set; }
    public string? SearchText { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid? CreatorId { get; set; }
} 
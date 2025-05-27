namespace QuizApp.Application.DTOs.Requests.QuestionRepo;

public record GetQuestionReposByCategoryRequest
{
    public Guid CategoryId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 
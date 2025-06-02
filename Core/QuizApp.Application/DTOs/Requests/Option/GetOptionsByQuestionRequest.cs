namespace QuizApp.Application.DTOs.Requests.Option;

public record GetOptionsByQuestionRequest
{
    public Guid QuestionId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 
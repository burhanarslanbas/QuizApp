namespace QuizApp.Application.DTOs.Requests.Option;

public record CreateOptionRequest
{
    public string OptionText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public Guid QuestionId { get; set; }
} 
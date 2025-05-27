namespace QuizApp.Application.DTOs.Requests.Option;

public record CreateOptionRequest
{
    public Guid QuestionId { get; set; }
    public string OptionText { get; set; }
    public bool IsCorrect { get; set; }
    public byte OrderIndex { get; set; }
} 
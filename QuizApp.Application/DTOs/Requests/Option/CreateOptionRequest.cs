namespace QuizApp.Application.DTOs.Requests.Option;

public record CreateOptionRequest
{
    public string OptionText { get; set; }
    public bool IsCorrect { get; set; }
} 
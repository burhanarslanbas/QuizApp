namespace QuizApp.Application.DTOs.Requests.Option;

public record UpdateOptionRequest
{
    public Guid Id { get; set; }
    public string OptionText { get; set; }
    public bool IsCorrect { get; set; }
} 
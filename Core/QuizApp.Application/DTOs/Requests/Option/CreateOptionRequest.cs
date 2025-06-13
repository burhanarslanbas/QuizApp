namespace QuizApp.Application.DTOs.Requests.Option;

public record CreateOptionRequest
{
    public Guid? QuestionId { get; set; }
    public string OptionText { get; set; } = default!;
    public bool IsCorrect { get; set; } = false;
    public byte OrderIndex { get; set; } = 0;
}
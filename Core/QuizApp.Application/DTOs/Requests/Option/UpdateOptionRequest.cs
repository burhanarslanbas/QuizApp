namespace QuizApp.Application.DTOs.Requests.Option;

public record UpdateOptionRequest
{
    public Guid Id { get; set; }
    public Guid? QuestionId { get; set; }
    public string OptionText { get; set; } = default!;
    public byte OrderIndex { get; set; }
    public bool IsCorrect { get; set; }

}
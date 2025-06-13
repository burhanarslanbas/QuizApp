namespace QuizApp.Application.DTOs.Responses.Option;

public record OptionResponse
{
    public Guid Id { get; set; }
    public Guid? QuestionId { get; set; }
    public string OptionText { get; set; } = default!;
    public byte OrderIndex { get; set; }
    public bool IsCorrect { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
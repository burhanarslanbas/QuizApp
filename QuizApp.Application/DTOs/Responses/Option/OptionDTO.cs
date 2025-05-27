namespace QuizApp.Application.DTOs.Responses.Option;

public record OptionDTO
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public Guid QuestionId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; }
}
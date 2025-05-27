namespace QuizApp.Application.DTOs.Responses.Option;

public record OptionDetailResponse
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public int Order { get; set; }
}
namespace QuizApp.Application.DTOs.Responses.UserAnswer;

public record UserAnswerResponse
{
    public Guid Id { get; set; }
    public Guid QuestionId { get; set; }
    public Guid? OptionId { get; set; }
    public Guid? QuizResultId { get; set; }
    public string? TextAnswer { get; set; }
    public bool IsCorrect { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
} 
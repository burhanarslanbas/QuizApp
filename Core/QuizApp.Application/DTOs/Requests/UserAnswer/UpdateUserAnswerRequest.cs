namespace QuizApp.Application.DTOs.Requests.UserAnswer;

public record UpdateUserAnswerRequest
{
    public Guid Id { get; set; }
    public Guid QuestionId { get; set; }
    public Guid? OptionId { get; set; }
    public Guid? QuizResultId { get; set; }
    public string? TextAnswer { get; set; }
    public bool IsCorrect { get; set; }
}

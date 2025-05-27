namespace QuizApp.Application.DTOs.Requests.UserAnswer;

public record UpdateUserAnswerRequest
{
    public int Id { get; init; }
    public int QuizResultId { get; init; }
    public int QuestionId { get; init; }
    public string Answer { get; init; } = string.Empty;
    public bool IsCorrect { get; init; }
}

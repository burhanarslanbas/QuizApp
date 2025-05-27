namespace QuizApp.Application.DTOs.Requests.UserAnswer;

public record CreateUserAnswerRequest
{
    public Guid UserId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid OptionId { get; set; }
    public string? AnswerText { get; set; }
}
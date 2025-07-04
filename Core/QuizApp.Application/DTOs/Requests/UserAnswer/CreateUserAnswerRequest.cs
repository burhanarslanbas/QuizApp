namespace QuizApp.Application.DTOs.Requests.UserAnswer;

public record CreateUserAnswerRequest
{
    public Guid QuestionId { get; set; }
    public Guid? OptionId { get; set; }
    public Guid? QuizResultId { get; set; }
    public string? TextAnswer { get; set; }
}
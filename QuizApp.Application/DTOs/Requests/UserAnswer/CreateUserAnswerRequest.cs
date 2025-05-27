namespace QuizApp.Application.DTOs.Requests.UserAnswer;

public record CreateUserAnswerRequest
{
    public Guid QuizResultId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid SelectedOptionId { get; set; }
    public DateTime AnsweredAt { get; set; }
}
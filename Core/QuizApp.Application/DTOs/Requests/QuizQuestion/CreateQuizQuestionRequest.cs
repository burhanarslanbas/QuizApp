namespace QuizApp.Application.DTOs.Requests.QuizQuestion;

public record CreateQuizQuestionRequest
{
    public Guid QuizId { get; set; }
    public Guid QuestionId { get; set; }
    public byte OrderIndex { get; set; }
}
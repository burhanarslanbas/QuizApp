namespace QuizApp.Application.DTOs.Requests.QuizQuestion;

public record UpdateQuizQuestionRequest
{
    public Guid Id { get; set; }
    public Guid QuizId { get; set; }
    public Guid QuestionId { get; set; }
    public byte OrderIndex { get; set; }
}
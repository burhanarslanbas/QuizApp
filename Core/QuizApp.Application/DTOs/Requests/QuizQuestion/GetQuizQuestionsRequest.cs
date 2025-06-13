namespace QuizApp.Application.DTOs.Requests.QuizQuestion;

public record GetQuizQuestionsRequest
{
    public Guid? QuizId { get; set; }
    public Guid? QuestionId { get; set; }
}
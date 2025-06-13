namespace QuizApp.Application.DTOs.Requests.QuizQuestion;

public record DeleteQuizQuestionRequest
{
    public Guid Id { get; set; }
}
namespace QuizApp.Application.DTOs.Requests.Quiz;

public record DeleteQuizRequest
{
    public Guid Id { get; set; }
}

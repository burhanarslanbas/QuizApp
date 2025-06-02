namespace QuizApp.Application.DTOs.Requests.Question;

public record DeleteQuestionRequest
{
    public Guid Id { get; set; }
} 
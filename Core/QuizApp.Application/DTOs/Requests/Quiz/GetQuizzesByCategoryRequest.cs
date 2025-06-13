namespace QuizApp.Application.DTOs.Requests.Quiz;

public record GetQuizzesByCategoryRequest
{
    public Guid CategoryId { get; set; }
}
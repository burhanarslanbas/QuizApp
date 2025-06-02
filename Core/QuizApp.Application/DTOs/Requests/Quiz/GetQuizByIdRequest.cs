namespace QuizApp.Application.DTOs.Requests.Quiz;

public record GetQuizByIdRequest
{
    public Guid Id { get; set; }
} 
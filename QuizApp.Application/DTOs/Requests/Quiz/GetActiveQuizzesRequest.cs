namespace QuizApp.Application.DTOs.Requests.Quiz;

public record GetActiveQuizzesRequest
{
    public bool IsActive { get; set; } = true;
} 
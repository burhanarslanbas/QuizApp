namespace QuizApp.Application.DTOs.Requests.Question;

public record GetQuestionByIdRequest
{
    public Guid Id { get; set; }
} 
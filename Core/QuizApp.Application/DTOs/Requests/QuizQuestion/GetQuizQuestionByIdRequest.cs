namespace QuizApp.Application.DTOs.Requests.QuizQuestion;

public record GetQuizQuestionByIdRequest
{
    public Guid Id { get; set; }
}
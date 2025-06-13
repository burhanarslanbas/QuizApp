namespace QuizApp.Application.DTOs.Requests.UserAnswer;

public record GetUserAnswerByIdRequest
{
    public Guid Id { get; set; }
}
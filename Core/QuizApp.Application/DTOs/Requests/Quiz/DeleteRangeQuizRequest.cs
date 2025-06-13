namespace QuizApp.Application.DTOs.Requests.Quiz;

public record DeleteRangeQuizRequest
{
    public List<Guid> Ids { get; set; }
}
namespace QuizApp.Application.DTOs.Requests.QuizResult;

public record DeleteRangeQuizResultRequest
{
    public List<Guid> Ids { get; set; }
}
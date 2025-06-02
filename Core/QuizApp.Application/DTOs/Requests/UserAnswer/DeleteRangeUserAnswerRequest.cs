namespace QuizApp.Application.DTOs.Requests.UserAnswer;

public record DeleteRangeUserAnswerRequest
{
    public List<Guid> Ids { get; set; }
}
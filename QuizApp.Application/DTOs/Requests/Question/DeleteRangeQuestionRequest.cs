namespace QuizApp.Application.DTOs.Requests.Question;

public record DeleteRangeQuestionRequest
{
    public List<Guid> Ids { get; set; }
} 
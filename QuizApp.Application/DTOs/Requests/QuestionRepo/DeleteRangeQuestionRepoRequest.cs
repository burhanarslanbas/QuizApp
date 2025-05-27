namespace QuizApp.Application.DTOs.Requests.QuestionRepo;

public class DeleteRangeQuestionRepoRequest
{
    public List<Guid> Ids { get; set; } = new();
}
namespace QuizApp.Application.DTOs.Requests.Quiz;

public class UpdateRangeQuizRequest
{
    public required List<Guid> Ids { get; set; }
    public required List<UpdateQuizRequest> Quizzes { get; set; }
}
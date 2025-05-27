namespace QuizApp.Application.DTOs.Requests.Question;

public class GetQuestionsRequest
{
    public Guid? QuizId { get; set; }
    public Guid? QuestionTypeId { get; set; }
    public bool IsActive { get; set; } = true;
} 
namespace QuizApp.Application.DTOs.Requests.QuestionRepo;

public class GetQuestionReposRequest
{
    public Guid? CreatorId { get; set; }
    public bool IsPublic { get; set; }
    public bool IsActive { get; set; } = true;
} 
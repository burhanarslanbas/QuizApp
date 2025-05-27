using QuizApp.Application.DTOs.Requests.Option;

namespace QuizApp.Application.DTOs.Requests.QuestionRepo;

public record UpdateQuestionRepoRequest
{
    public Guid Id { get; set; }
    public string QuestionText { get; set; }
    public string Explanation { get; set; }
    public int DifficultyLevel { get; set; }
    public Guid CategoryId { get; set; }
    public List<UpdateOptionRequest> Options { get; set; }
    public bool IsActive { get; set; }
} 
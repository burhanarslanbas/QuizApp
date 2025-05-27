
using QuizApp.Application.DTOs.Requests.Option;

namespace QuizApp.Application.DTOs.Requests.QuestionRepo;

public record CreateQuestionRepoRequest
{
    public string QuestionText { get; set; }
    public string Explanation { get; set; }
    public int DifficultyLevel { get; set; }
    public Guid CategoryId { get; set; }
    public List<CreateOptionRequest> Options { get; set; }
    public bool IsActive { get; set; } = true;
} 
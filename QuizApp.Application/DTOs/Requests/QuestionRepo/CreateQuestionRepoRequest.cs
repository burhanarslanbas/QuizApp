using QuizApp.Application.DTOs.Requests.Option;

namespace QuizApp.Application.DTOs.Requests.QuestionRepo;

public record CreateQuestionRepoRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int MaxQuestions { get; set; } = 10;
    public bool IsPublic { get; set; } = false;
    public bool IsActive { get; set; } = true;
} 
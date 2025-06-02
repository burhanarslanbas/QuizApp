using QuizApp.Application.DTOs.Responses.Option;

namespace QuizApp.Application.DTOs.Responses.QuestionRepo;

public record QuestionRepoDTO
{
    public Guid Id { get; set; }
    public string QuestionText { get; set; }
    public string Explanation { get; set; }
    public Guid CreatorId { get; set; }
    public int MaxQuestions { get; set; }
    public bool IsPublic { get; set; }
    public int QuestionCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
} 
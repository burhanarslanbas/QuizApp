namespace QuizApp.Application.DTOs.Responses.QuestionRepo;

public record QuestionRepoResponse
{
    public Guid Id { get; set; }
    public string QuestionText { get; set; } = default!;
    public string? Explanation { get; set; }
    public Guid CreatorId { get; set; }
    public int MaxQuestions { get; set; }
    public bool IsPublic { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
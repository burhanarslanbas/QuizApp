namespace QuizApp.Application.DTOs.Responses.Category;

public record CategoryDetailResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<QuizSummaryResponse> Quizzes { get; set; }
}

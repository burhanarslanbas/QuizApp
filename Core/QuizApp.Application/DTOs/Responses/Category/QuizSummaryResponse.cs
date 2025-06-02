namespace QuizApp.Application.DTOs.Responses.Category;

public record QuizSummaryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int TotalQuestions { get; set; }
    public int Duration { get; set; }
    public bool IsActive { get; set; }
} 
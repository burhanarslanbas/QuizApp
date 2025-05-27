namespace QuizApp.Application.DTOs.Requests.Quiz;

public record UpdateQuizRequest
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TimeSpan Duration { get; set; }
    public int PassingScore { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int MaxAttempts { get; set; }
    public bool ShowResults { get; set; }
} 
namespace QuizApp.Application.DTOs.Requests.QuizResult;

public record GetQuizResultsRequest
{
    public Guid? QuizId { get; set; }
    public Guid? UserId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? MinScore { get; set; }
    public int? MaxScore { get; set; }
    public TimeSpan? MinDuration { get; set; }
    public TimeSpan? MaxDuration { get; set; }
    public string? Status { get; set; }
    public bool IsActive { get; set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
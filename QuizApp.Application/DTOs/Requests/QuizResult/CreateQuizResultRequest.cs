namespace QuizApp.Application.DTOs.Requests.QuizResult;

public record CreateQuizResultRequest
{
    public Guid UserId { get; set; }
    public Guid QuizId { get; set; }
    public int Score { get; set; }
    public DateTime CompletedAt { get; set; }
    public bool IsPassed { get; set; }
} 
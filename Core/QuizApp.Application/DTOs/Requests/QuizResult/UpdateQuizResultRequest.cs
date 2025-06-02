namespace QuizApp.Application.DTOs.Requests.QuizResult;

public record UpdateQuizResultRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid QuizId { get; set; }
    public int Score { get; set; }
    public DateTime CompletedAt { get; set; }
    public bool IsPassed { get; set; }
} 
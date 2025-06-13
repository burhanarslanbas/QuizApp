namespace QuizApp.Application.DTOs.Requests.QuizResult;

public record CreateQuizResultRequest
{
    public Guid QuizId { get; set; }
    public Guid UserId { get; set; }
    public int Score { get; set; }
    public DateTime? EndTime { get; set; }
    public bool IsCompleted { get; set; }
}
namespace QuizApp.Application.DTOs.Responses.QuizResult;

public record QuizResultResponse
{
    public Guid Id { get; set; }
    public Guid QuizId { get; set; }
    public Guid UserId { get; set; }
    public int Score { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
} 
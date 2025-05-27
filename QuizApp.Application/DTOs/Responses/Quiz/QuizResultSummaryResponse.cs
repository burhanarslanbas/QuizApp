namespace QuizApp.Application.DTOs.Responses.Quiz;

public record QuizResultSummaryResponse
{
    public Guid Id { get; set; }
    public string QuizTitle { get; set; }
    public int Score { get; set; }
    public DateTime CompletedAt { get; set; }
    public string Status { get; set; }
}
using QuizApp.Application.DTOs.Responses.UserAnswer;

namespace QuizApp.Application.DTOs.Responses.QuizResult;

public record QuizResultDetailResponse
{
    public Guid Id { get; set; }
    public Guid QuizId { get; set; }
    public string QuizTitle { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public int Score { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; }
    public bool IsActive { get; set; }
    public List<UserAnswerDetailResponse> UserAnswers { get; set; }
}
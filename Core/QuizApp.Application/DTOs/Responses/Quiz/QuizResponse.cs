using QuizApp.Application.DTOs.Responses.Question;

namespace QuizApp.Application.DTOs.Responses.Quiz;

public class QuizResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Duration { get; set; }
    public int MaxAttempts { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<QuestionResponse> Questions { get; set; }
}
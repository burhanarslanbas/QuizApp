using QuizApp.Application.DTOs.Requests.QuizQuestion;

namespace QuizApp.Application.DTOs.Requests.Quiz;

public record UpdateQuizRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Duration { get; set; }
    public int MaxAttempts { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public List<UpdateQuizQuestionRequest> Questions { get; set; }
}
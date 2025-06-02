using QuizApp.Application.DTOs.Responses.Question;

namespace QuizApp.Application.DTOs.Responses.Quiz;

public record QuizDetailResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int Duration { get; set; }
    public int TotalQuestions { get; set; }
    public int PassingScore { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<QuestionDetailResponse> Questions { get; set; }
}

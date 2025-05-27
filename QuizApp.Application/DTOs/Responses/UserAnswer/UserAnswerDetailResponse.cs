namespace QuizApp.Application.DTOs.Responses.UserAnswer;

public record UserAnswerDetailResponse
{
    public Guid Id { get; set; }
    public Guid QuestionId { get; set; }
    public string QuestionText { get; set; }
    public Guid SelectedOptionId { get; set; }
    public string SelectedOptionText { get; set; }
    public bool IsCorrect { get; set; }
    public Guid CorrectOptionId { get; set; }
    public string CorrectOptionText { get; set; }
}
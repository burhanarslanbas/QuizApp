namespace QuizApp.Application.DTOs.Requests.UserAnswer;

public class GetUserAnswersRequest
{
    public int? QuizResultId { get; set; }
    public int? QuestionId { get; set; }
    public bool? IsCorrect { get; set; }
    public bool IsActive { get; set; } = true;
} 
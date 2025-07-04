using QuizApp.Domain.Enums;

namespace QuizApp.Application.DTOs.Requests.Question;

public class GetQuestionsRequest
{
    public Guid? QuizId { get; set; }
    public QuestionType? QuestionType { get; set; }
    public bool? IsActive { get; set; }
}
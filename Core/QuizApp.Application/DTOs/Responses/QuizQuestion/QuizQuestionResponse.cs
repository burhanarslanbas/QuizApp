namespace QuizApp.Application.DTOs.Responses.QuizQuestion;

public record QuizQuestionResponse
{
    public Guid Id { get; set; }
    public Guid QuizId { get; set; }
    public Guid QuestionId { get; set; }
    public byte OrderIndex { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
} 
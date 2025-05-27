using QuizApp.Domain.Entities;

namespace QuizApp.Application.DTOs.Responses.Option;

public class OptionDTO
{
    public Guid Id { get; set; }
    public string OptionText { get; set; }
    public bool IsCorrect { get; set; }
    public Guid QuestionId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; }

    public static OptionDTO FromEntity(QuizApp.Domain.Entities.Option option)
    {
        return new OptionDTO
        {
            Id = option.Id,
            OptionText = option.OptionText,
            IsCorrect = option.IsCorrect,
            QuestionId = option.QuestionId
        };
    }
}
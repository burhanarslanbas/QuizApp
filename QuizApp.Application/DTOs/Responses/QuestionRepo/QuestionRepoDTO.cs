using QuizApp.Application.DTOs.Responses.Option;

namespace QuizApp.Application.DTOs.Responses.QuestionRepo;

public record QuestionRepoDTO
{
    public Guid Id { get; set; }
    public string QuestionText { get; set; }
    public string Explanation { get; set; }
    public int DifficultyLevel { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public List<OptionDTO> Options { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; }
} 
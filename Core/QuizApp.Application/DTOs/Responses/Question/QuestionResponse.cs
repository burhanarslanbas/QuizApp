using QuizApp.Application.DTOs.Responses.Option;
using QuizApp.Domain.Enums;

namespace QuizApp.Application.DTOs.Responses.Question
{
    public record QuestionResponse
    {
        public Guid Id { get; set; }
        public Guid? QuizId { get; set; }
        public Guid? QuestionRepoId { get; set; }
        public Guid CreatorId { get; set; }
        public QuestionType QuestionType { get; set; }
        public string QuestionText { get; set; } = default!;
        public string? Explanation { get; set; }
        public int OrderIndex { get; set; }
        public int Points { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<OptionResponse> Options { get; set; } = default!;
    }
}
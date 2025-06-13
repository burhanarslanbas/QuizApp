using QuizApp.Application.DTOs.Requests.Option;
using QuizApp.Domain.Enums;

namespace QuizApp.Application.DTOs.Requests.Question
{
    public record CreateQuestionRequest
    {
        public Guid? QuestionRepoId { get; set; }
        public string QuestionText { get; set; } = default!;
        public QuestionType QuestionType { get; set; }
        public int Points { get; set; } = 1;
        public int OrderIndex { get; set; }
        public string? Explanation { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public required List<CreateOptionRequest> Options { get; set; }
    }
}
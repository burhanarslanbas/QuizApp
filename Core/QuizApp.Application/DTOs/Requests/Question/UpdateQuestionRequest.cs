using QuizApp.Application.DTOs.Requests.Option;
using QuizApp.Domain.Entities;
using QuizApp.Domain.Enums;

namespace QuizApp.Application.DTOs.Requests.Question
{
    public record UpdateQuestionRequest
    {
        public Guid Id { get; set; }
        public string QuestionText { get; set; }
        public int Points { get; set; }
        public Guid QuizId { get; set; }
        public Guid? QuestionRepoId { get; set; }
        public Guid QuestionTypeId { get; set; }
        public int OrderIndex { get; set; }
        public List<CreateOptionRequest> Options { get; set; }

        public Domain.Entities.Question ToEntity()
        {
            return new Domain.Entities.Question
            {
                Id = Id,
                QuestionText = QuestionText,
                Points = Points,
                QuizId = QuizId,
                QuestionRepoId = QuestionRepoId,
                QuestionTypeId = QuestionTypeId,
                OrderIndex = OrderIndex
            };
        }
    }
}
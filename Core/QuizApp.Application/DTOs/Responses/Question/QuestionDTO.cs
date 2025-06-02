using System;
using QuizApp.Application.DTOs.Responses.Option;

namespace QuizApp.Application.DTOs.Responses.Question
{
    public class QuestionDTO
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string Explanation { get; set; }
        public int Points { get; set; }
        public Guid QuizId { get; set; }
        public Guid? QuestionRepoId { get; set; }
        public Guid CreatorId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<OptionDTO> Options { get; set; }
    }
} 
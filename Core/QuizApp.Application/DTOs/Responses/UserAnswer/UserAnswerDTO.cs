using System;
using QuizApp.Application.DTOs.Responses.Option;
using QuizApp.Application.DTOs.Responses.Question;
using QuizApp.Application.DTOs.Responses.QuizResult;

namespace QuizApp.Application.DTOs.Responses.UserAnswer
{
    public class UserAnswerDTO
    {
        public Guid Id { get; set; }
        public Guid QuizResultId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid SelectedOptionId { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime AnsweredAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public QuizResultDTO QuizResult { get; set; }
        public QuestionDTO Question { get; set; }
        public OptionDTO SelectedOption { get; set; }
    }
} 
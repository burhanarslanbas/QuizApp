using QuizApp.Application.DTOs.Responses.Quiz;

using QuizApp.Application.DTOs.Responses.UserAnswer;

namespace QuizApp.Application.DTOs.Responses.QuizResult
{
    public class QuizResultDTO
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public Guid StudentId { get; set; }
        public int Score { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public QuizDTO Quiz { get; set; }
        public List<UserAnswerDTO> UserAnswers { get; set; }
    }
}
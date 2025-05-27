using QuizApp.Application.DTOs.Requests.Option;

namespace QuizApp.Application.DTOs.Requests.Question;

public record UpdateQuestionRequest
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public string Explanation { get; set; }
    public int Points { get; set; }
    public Guid QuizId { get; set; }
    public Guid? QuestionRepoId { get; set; }
    public bool IsActive { get; set; }
    public List<UpdateOptionRequest> Options { get; set; }
}
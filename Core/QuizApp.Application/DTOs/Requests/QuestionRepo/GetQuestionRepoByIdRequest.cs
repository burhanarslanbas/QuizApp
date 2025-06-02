namespace QuizApp.Application.DTOs.Requests.QuestionRepo;

public record GetQuestionRepoByIdRequest
{
    public Guid Id { get; set; }
} 
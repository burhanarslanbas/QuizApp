namespace QuizApp.Application.DTOs.Requests.QuestionRepo;

public record CreateQuestionRepoRequest(Guid CreatorId, string Name, string? Description, bool IsActive, int MaxQuestions, bool IsPublic);
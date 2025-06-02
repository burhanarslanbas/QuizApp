namespace QuizApp.Application.DTOs.Requests.QuestionRepo;

public record CreateQuestionRepoRequest(Guid CreatorId, string Name, string Description, int MaxQuestions, bool IsPublic);
namespace QuizApp.Application.DTOs.Requests.QuestionRepo;

public record GetQuestionReposRequest(Guid? CreatorId, bool IsPublic, bool IsActive);

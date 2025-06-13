namespace QuizApp.Application.DTOs.Requests.QuestionRepo;

public record GetQuestionReposByUserRequest(
    Guid UserId,
    int Page = 1,
    int PageSize = 10
); 
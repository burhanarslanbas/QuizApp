namespace QuizApp.Application.DTOs.Requests.Question;

public record GetQuestionsByRepoRequest(
    Guid QuestionRepoId,
    int Page = 1,
    int PageSize = 10
); 
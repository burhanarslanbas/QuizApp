namespace QuizApp.Application.DTOs.Requests.Quiz;

public record GetQuizzesByUserRequest(
    Guid UserId,
    int Page = 1,
    int PageSize = 10
); 
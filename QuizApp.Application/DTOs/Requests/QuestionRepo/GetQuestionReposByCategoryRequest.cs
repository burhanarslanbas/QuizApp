namespace QuizApp.Application.DTOs.Requests.QuestionRepo;

public record GetQuestionReposByCategoryRequest(
    Guid CategoryId
);
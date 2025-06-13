namespace QuizApp.Application.DTOs.Requests.QuestionRepo;

public record DeleteRangeQuestionRepoRequest(List<DeleteQuestionRepoRequest> DeleteQuestionRepos);
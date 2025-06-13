namespace QuizApp.Application.DTOs.Requests.Question;

public record CreateRangeQuestionRequest(List<CreateQuestionRequest> Questions);
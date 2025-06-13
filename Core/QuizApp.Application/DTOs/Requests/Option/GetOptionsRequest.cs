namespace QuizApp.Application.DTOs.Requests.Option;

public record GetOptionsRequest(
    int Page = 1,
    int PageSize = 10
); 
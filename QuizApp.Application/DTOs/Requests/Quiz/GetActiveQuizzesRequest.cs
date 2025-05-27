namespace QuizApp.Application.DTOs.Requests.Quiz;
 
public record GetActiveQuizzesRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 
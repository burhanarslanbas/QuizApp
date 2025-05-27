namespace QuizApp.Application.DTOs.Requests.User;

public record GetUsersRequest
{
    public string? SearchText { get; set; }
    public bool? IsActive { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 
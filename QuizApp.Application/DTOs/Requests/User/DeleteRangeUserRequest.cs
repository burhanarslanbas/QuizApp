namespace QuizApp.Application.DTOs.Requests.User;

public record DeleteRangeUserRequest
{
    public List<Guid> Ids { get; set; }
}
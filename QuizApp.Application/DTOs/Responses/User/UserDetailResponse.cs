using QuizApp.Application.DTOs.Responses.Quiz;

namespace QuizApp.Application.DTOs.Responses.User;

public record UserDetailResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<QuizResultSummaryResponse> RecentQuizResults { get; set; }
}

namespace QuizApp.Application.DTOs.Requests.Option;

public record GetOptionByIdRequest
{
    public Guid Id { get; set; }
}
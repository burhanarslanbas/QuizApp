using QuizApp.Application.DTOs.Responses.Option;

namespace QuizApp.Application.DTOs.Responses.Question;

public record QuestionDetailResponse
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public int Order { get; set; }
    public List<OptionDetailResponse> Options { get; set; }
}

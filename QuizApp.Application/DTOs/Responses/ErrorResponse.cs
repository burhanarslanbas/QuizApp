namespace QuizApp.Application.DTOs.Responses
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string? Details { get; set; }
    }
} 
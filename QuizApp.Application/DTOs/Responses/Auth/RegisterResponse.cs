namespace QuizApp.Application.DTOs.Responses.Auth
{
    public class RegisterResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
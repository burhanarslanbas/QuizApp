using System.Collections.Generic;

namespace QuizApp.Application.DTOs.Requests.Quiz;

public class CreateRangeQuizRequest
{
    public required List<CreateQuizRequest> Quizzes { get; set; }
} 
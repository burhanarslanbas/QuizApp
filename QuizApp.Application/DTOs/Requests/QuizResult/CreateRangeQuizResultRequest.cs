using System.Collections.Generic;

namespace QuizApp.Application.DTOs.Requests.QuizResult;

public class CreateRangeQuizResultRequest
{
    public required List<CreateQuizResultRequest> QuizResults { get; set; }
} 
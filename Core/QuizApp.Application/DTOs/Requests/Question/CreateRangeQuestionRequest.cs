using System.Collections.Generic;

namespace QuizApp.Application.DTOs.Requests.Question;

public class CreateRangeQuestionRequest
{
    public required List<CreateQuestionRequest> Questions { get; set; }
} 
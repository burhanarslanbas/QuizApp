using System.Collections.Generic;

namespace QuizApp.Application.DTOs.Requests.Question;

public class UpdateRangeQuestionRequest
{
    public required List<Guid> Ids { get; set; }
    public required List<UpdateQuestionRequest> Questions { get; set; }
} 
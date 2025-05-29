using System.Collections.Generic;

namespace QuizApp.Application.DTOs.Requests.QuizResult;

public class UpdateRangeQuizResultRequest
{
    public required List<Guid> Ids { get; set; }
    public required List<UpdateQuizResultRequest> QuizResults { get; set; }
} 
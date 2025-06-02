using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.QuizResult;
using QuizApp.Application.Services;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class QuizResultsController : ControllerBase
{
    private readonly IQuizResultService _quizResultService;

    public QuizResultsController(IQuizResultService quizResultService)
    {
        _quizResultService = quizResultService;
    }

    /// <summary>
    /// Id ile quiz sonucu getirir
    /// </summary>
    [HttpGet("{quizResultId}")]
    public async Task<IActionResult> GetQuizResultById([FromRoute] Guid quizResultId)
    {
        var result = await _quizResultService.GetByIdAsync(quizResultId);
        return Ok(result);
    }

    /// <summary>
    /// Tüm quiz sonuçlarını getirir
    /// </summary>
    [HttpGet]
    public IActionResult GetAllQuizResults()
    {
        var result = _quizResultService.GetAll();
        return Ok(result);
    }

    /// <summary>
    /// Yeni quiz sonucu oluşturur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateQuizResult([FromBody] CreateQuizResultRequest request)
    {
        var result = await _quizResultService.CreateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Quiz sonucu günceller
    /// </summary>
    [HttpPut]
    public IActionResult UpdateQuizResult([FromBody] UpdateQuizResultRequest request)
    {
        var result = _quizResultService.Update(request);
        return Ok(result);
    }

    /// <summary>
    /// Quiz sonucu siler
    /// </summary>
    [HttpDelete("{quizResultId}")]
    public async Task<IActionResult> DeleteQuizResult([FromRoute] Guid quizResultId)
    {
        await _quizResultService.DeleteAsync(quizResultId);
        return Ok();
    }

    /// <summary>
    /// Çoklu quiz sonucu siler
    /// </summary>
    [HttpDelete("range")]
    public IActionResult DeleteRange([FromBody] List<Guid> ids)
    {
        var result = _quizResultService.DeleteRange(ids);
        return Ok(result);
    }

    /// <summary>
    /// Giriş yapan öğretmenin kendi quizine ait sonuçları getirir
    /// </summary>
    [HttpGet("by-quiz/{quizId}")]
    [Authorize(Roles = "Teacher")]
    public IActionResult GetResultsByQuiz([FromRoute] Guid quizId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();
        var results = _quizResultService.GetByQuizIdAndOwner(quizId, Guid.Parse(userId));
        return Ok(results);
    }
}
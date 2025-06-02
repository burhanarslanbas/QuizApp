using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Quiz;
using QuizApp.Application.DTOs.Responses.Quiz;
using QuizApp.Application.Services;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class QuizzesController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuizzesController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    /// <summary>
    /// Id ile quiz getirir
    /// </summary>
    [HttpGet("{quizId}")]
    public async Task<IActionResult> GetQuizById([FromRoute] Guid quizId)
    {
        var result = await _quizService.GetByIdAsync(new GetQuizByIdRequest { Id = quizId });
        return Ok(result);
    }

    /// <summary>
    /// Tüm quizleri getirir
    /// </summary>
    [HttpGet]
    public IActionResult GetAllQuizzes([FromQuery] GetQuizzesRequest request)
    {
        var result = _quizService.GetAll(request);
        return Ok(result);
    }

    /// <summary>
    /// Yeni quiz oluşturur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizRequest request)
    {
        var result = await _quizService.CreateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Quiz günceller
    /// </summary>
    [HttpPut]
    public IActionResult UpdateQuiz([FromBody] UpdateQuizRequest request)
    {
        var result = _quizService.Update(request);
        return Ok(result);
    }

    /// <summary>
    /// Quiz siler
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> DeleteQuiz([FromBody] DeleteQuizRequest request)
    {
        await _quizService.DeleteAsync(request);
        return Ok();
    }

    /// <summary>
    /// Çoklu quiz siler
    /// </summary>
    [HttpDelete("range")]
    public IActionResult DeleteRange([FromBody] DeleteRangeQuizRequest request)
    {
        var result = _quizService.DeleteRange(request);
        return Ok(result);
    }

    /// <summary>
    /// Giriş yapan öğretmenin kendi oluşturduğu quizleri getirir
    /// </summary>
    [HttpGet("my")]
    [Authorize(Roles = "Teacher")]
    public IActionResult GetMyQuizzes([FromQuery] GetQuizzesRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();
        request.CreatorId = Guid.Parse(userId);
        var result = _quizService.GetAll(request);
        return Ok(result);
    }
} 
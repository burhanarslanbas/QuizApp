using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Quiz;
using QuizApp.Application.DTOs.Responses.Quiz;
using QuizApp.Application.Services;

namespace QuizApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizController : ControllerBase
{
    private readonly IQuizService _quizService;
    public QuizController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    /// <summary>
    /// Tüm quizleri getirir.
    /// </summary>
    [HttpGet("GetAll")]
    public ActionResult<List<QuizDetailResponse>> GetAll()
    {
        var quizzes = _quizService.GetAll(new GetQuizzesRequest());
        return Ok(quizzes);
    }

    /// <summary>
    /// Belirli bir quiz bilgisini getirir.
    /// </summary>
    [HttpGet("GetById/{id}")]
    public async Task<ActionResult<QuizDetailResponse>> GetById(Guid id)
    {
        var quiz = await _quizService.GetByIdAsync(new GetQuizByIdRequest { Id = id });
        return Ok(quiz);
    }

    /// <summary>
    /// Yeni bir quiz oluşturur.
    /// </summary>
    [HttpPost("Create")]
    public async Task<ActionResult<QuizDetailResponse>> Create([FromBody] CreateQuizRequest request)
    {
        var result = await _quizService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Quiz bilgisini günceller.
    /// </summary>
    [HttpPut("Update")]
    public ActionResult<QuizDetailResponse> Update([FromBody] UpdateQuizRequest request)
    {
        var result = _quizService.Update(request);
        return Ok(result);
    }

    /// <summary>
    /// Belirli bir quizi siler.
    /// </summary>
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _quizService.DeleteAsync(new DeleteQuizRequest { Id = id });
        return NoContent();
    }

    /// <summary>
    /// Birden fazla quizi siler.
    /// </summary>
    [HttpPost("DeleteRange")]
    public IActionResult DeleteRange([FromBody] DeleteRangeQuizRequest request)
    {
        var result = _quizService.DeleteRange(request);
        return result ? NoContent() : BadRequest();
    }
} 
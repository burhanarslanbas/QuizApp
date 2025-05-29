using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.QuizResult;
using QuizApp.Application.DTOs.Responses.QuizResult;
using QuizApp.Application.Services;

namespace QuizApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizResultController : ControllerBase
{
    private readonly IQuizResultService _quizResultService;
    public QuizResultController(IQuizResultService quizResultService)
    {
        _quizResultService = quizResultService;
    }

    /// <summary>
    /// Tüm quiz sonuçlarını getirir.
    /// </summary>
    [HttpGet("GetAll")]
    public ActionResult<List<QuizResultDTO>> GetAll()
    {
        var quizResults = _quizResultService.GetAll();
        return Ok(quizResults);
    }

    /// <summary>
    /// Belirli bir quiz sonucu bilgisini getirir.
    /// </summary>
    [HttpGet("GetById/{id}")]
    public async Task<ActionResult<QuizResultDTO>> GetById(Guid id)
    {
        var quizResult = await _quizResultService.GetByIdAsync(id);
        return Ok(quizResult);
    }

    /// <summary>
    /// Yeni bir quiz sonucu oluşturur.
    /// </summary>
    [HttpPost("Create")]
    public async Task<ActionResult<bool>> Create([FromBody] CreateQuizResultRequest request)
    {
        var result = await _quizResultService.CreateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Birden fazla quiz sonucu oluşturur.
    /// </summary>
    [HttpPost("CreateRange")]
    public async Task<ActionResult<List<QuizResultDTO>>> CreateRange([FromBody] List<CreateQuizResultRequest> request)
    {
        var result = await _quizResultService.CreateRangeAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Quiz sonucu bilgisini günceller.
    /// </summary>
    [HttpPut("Update")]
    public ActionResult<bool> Update([FromBody] UpdateQuizResultRequest request)
    {
        var result = _quizResultService.Update(request);
        return Ok(result);
    }

    /// <summary>
    /// Belirli bir quiz sonucunu siler.
    /// </summary>
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _quizResultService.DeleteAsync(id);
        return result ? NoContent() : BadRequest();
    }

    /// <summary>
    /// Birden fazla quiz sonucunu siler.
    /// </summary>
    [HttpPost("DeleteRange")]
    public IActionResult DeleteRange([FromBody] List<Guid> ids)
    {
        var result = _quizResultService.DeleteRange(ids);
        return result ? NoContent() : BadRequest();
    }
} 
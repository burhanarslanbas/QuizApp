using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Question;
using QuizApp.Application.DTOs.Responses.Question;
using QuizApp.Application.Services;

namespace QuizApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;
    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    /// <summary>
    /// Tüm soruları getirir.
    /// </summary>
    [HttpGet("GetAll")]
    public ActionResult<List<QuestionDetailResponse>> GetAll()
    {
        var questions = _questionService.GetAll(new GetQuestionsRequest());
        return Ok(questions);
    }

    /// <summary>
    /// Belirli bir soru bilgisini getirir.
    /// </summary>
    [HttpGet("GetById/{id}")]
    public async Task<ActionResult<QuestionDetailResponse>> GetById(Guid id)
    {
        var question = await _questionService.GetByIdAsync(new GetQuestionByIdRequest { Id = id });
        return Ok(question);
    }

    /// <summary>
    /// Yeni bir soru oluşturur.
    /// </summary>
    [HttpPost("Create")]
    public async Task<ActionResult<QuestionDetailResponse>> Create([FromBody] CreateQuestionRequest request)
    {
        var result = await _questionService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Soru bilgisini günceller.
    /// </summary>
    [HttpPut("Update")]
    public ActionResult<QuestionDetailResponse> Update([FromBody] UpdateQuestionRequest request)
    {
        var result = _questionService.Update(request);
        return Ok(result);
    }

    /// <summary>
    /// Belirli bir soruyu siler.
    /// </summary>
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _questionService.DeleteAsync(new DeleteQuestionRequest { Id = id });
        return NoContent();
    }

    /// <summary>
    /// Birden fazla soruyu siler.
    /// </summary>
    [HttpPost("DeleteRange")]
    public IActionResult DeleteRange([FromBody] DeleteRangeQuestionRequest request)
    {
        var result = _questionService.DeleteRange(request);
        return result ? NoContent() : BadRequest();
    }
} 
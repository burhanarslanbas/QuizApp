using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Question;
using QuizApp.Application.DTOs.Responses.Question;
using QuizApp.Application.Services;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionsController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    /// <summary>
    /// Id ile soru getirir
    /// </summary>
    [HttpGet("{questionId}")]
    public async Task<IActionResult> GetQuestionById([FromRoute] Guid questionId)
    {
        var result = await _questionService.GetByIdAsync(new GetQuestionByIdRequest { Id = questionId });
        return Ok(result);
    }

    /// <summary>
    /// Tüm soruları getirir
    /// </summary>
    [HttpGet]
    public IActionResult GetAllQuestions([FromQuery] GetQuestionsRequest request)
    {
        var result = _questionService.GetAll(request);
        return Ok(result);
    }

    /// <summary>
    /// Yeni soru oluşturur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest request)
    {
        var result = await _questionService.CreateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Soru günceller
    /// </summary>
    [HttpPut]
    public IActionResult UpdateQuestion([FromBody] UpdateQuestionRequest request)
    {
        var result = _questionService.Update(request);
        return Ok(result);
    }

    /// <summary>
    /// Soru siler
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> DeleteQuestion([FromBody] DeleteQuestionRequest request)
    {
        await _questionService.DeleteAsync(request);
        return Ok();
    }

    /// <summary>
    /// Çoklu soru siler
    /// </summary>
    [HttpDelete("range")]
    public IActionResult DeleteRange([FromBody] DeleteRangeQuestionRequest request)
    {
        var result = _questionService.DeleteRange(request);
        return Ok(result);
    }
} 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Question;
using QuizApp.Application.DTOs.Responses.Question;
using QuizApp.Application.Services;
using System.Security.Claims;

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
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _questionService.CreateAsync(request, Guid.Parse(userId));
        return Ok(result);
    }

    /// <summary>
    /// Soru günceller
    /// </summary>
    [HttpPut]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _questionService.Update(request, Guid.Parse(userId));
        return Ok(result);
    }

    /// <summary>
    /// Soru siler
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> DeleteQuestion([FromRoute] DeleteQuestionRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        await _questionService.DeleteAsync(request, Guid.Parse(userId));
        return Ok();
    }

    /// <summary>
    /// Çoklu soru siler
    /// </summary>
    [HttpDelete("range")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> DeleteRange([FromBody] DeleteRangeQuestionRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _questionService.DeleteRange(request, Guid.Parse(userId));
        return Ok(result);
    }
} 
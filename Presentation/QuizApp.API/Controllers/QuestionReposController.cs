using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.QuestionRepo;
using QuizApp.Application.DTOs.Responses.QuestionRepo;
using QuizApp.Application.Services;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class QuestionReposController : ControllerBase
{
    private readonly IQuestionRepoService _questionRepoService;

    public QuestionReposController(IQuestionRepoService questionRepoService)
    {
        _questionRepoService = questionRepoService;
    }

    /// <summary>
    /// Id ile soru deposu getirir
    /// </summary>
    [HttpGet("{questionRepoId}")]
    public async Task<IActionResult> GetQuestionRepoById([FromRoute] Guid questionRepoId)
    {
        var result = await _questionRepoService.GetByIdAsync(new GetQuestionRepoByIdRequest { Id = questionRepoId });
        return Ok(result);
    }

    /// <summary>
    /// Tüm soru depolarını getirir
    /// </summary>
    [HttpGet]
    public IActionResult GetAllQuestionRepos([FromQuery] GetQuestionReposRequest request)
    {
        var result = _questionRepoService.GetAll(request);
        return Ok(result);
    }

    /// <summary>
    /// Yeni soru deposu oluşturur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateQuestionRepo([FromBody] CreateQuestionRepoRequest request)
    {
        var result = await _questionRepoService.CreateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Soru deposu günceller
    /// </summary>
    [HttpPut]
    public IActionResult UpdateQuestionRepo([FromBody] UpdateQuestionRepoRequest request)
    {
        var result = _questionRepoService.Update(request);
        return Ok(result);
    }

    /// <summary>
    /// Soru deposu siler
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> DeleteQuestionRepo([FromBody] DeleteQuestionRepoRequest request)
    {
        await _questionRepoService.DeleteAsync(request);
        return Ok();
    }

    /// <summary>
    /// Çoklu soru deposu siler
    /// </summary>
    [HttpDelete("range")]
    public IActionResult DeleteRange([FromBody] DeleteRangeQuestionRepoRequest request)
    {
        var result = _questionRepoService.DeleteRange(request);
        return Ok(result);
    }
} 
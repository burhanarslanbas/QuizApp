using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.QuestionRepo;
using QuizApp.Application.DTOs.Responses.Question;
using QuizApp.Application.Services;

namespace QuizApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionRepoController : ControllerBase
{
    private readonly IQuestionRepoService _questionRepoService;
    public QuestionRepoController(IQuestionRepoService questionRepoService)
    {
        _questionRepoService = questionRepoService;
    }

    /// <summary>
    /// Tüm soru havuzlarını getirir.
    /// </summary>
    [HttpGet("GetAll")]
    public ActionResult<List<QuestionDetailResponse>> GetAll([FromQuery] GetQuestionReposRequest request)
    {
        var questionRepos = _questionRepoService.GetAll(request);
        return Ok(questionRepos);
    }

    /// <summary>
    /// Belirli bir soru havuzu bilgisini getirir.
    /// </summary>
    [HttpGet("GetById/{id}")]
    public async Task<ActionResult<QuestionDetailResponse>> GetById(Guid id)
    {
        var questionRepo = await _questionRepoService.GetByIdAsync(new GetQuestionRepoByIdRequest { Id = id });
        return Ok(questionRepo);
    }

    /// <summary>
    /// Kategoriye göre soru havuzlarını getirir.
    /// </summary>
    [HttpGet("GetByCategory")]
    public ActionResult<List<QuestionDetailResponse>> GetByCategory([FromQuery] GetQuestionReposByCategoryRequest request)
    {
        var questionRepos = _questionRepoService.GetByCategory(request);
        return Ok(questionRepos);
    }

    /// <summary>
    /// Yeni bir soru havuzu oluşturur.
    /// </summary>
    [HttpPost("Create")]
    public async Task<ActionResult<QuestionDetailResponse>> Create([FromBody] CreateQuestionRepoRequest request)
    {
        var result = await _questionRepoService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Soru havuzu bilgisini günceller.
    /// </summary>
    [HttpPut("Update")]
    public ActionResult<QuestionDetailResponse> Update([FromBody] UpdateQuestionRepoRequest request)
    {
        var result = _questionRepoService.Update(request);
        return Ok(result);
    }

    /// <summary>
    /// Belirli bir soru havuzunu siler.
    /// </summary>
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _questionRepoService.DeleteAsync(new DeleteQuestionRepoRequest { Id = id });
        return NoContent();
    }

    /// <summary>
    /// Birden fazla soru havuzunu siler.
    /// </summary>
    [HttpPost("DeleteRange")]
    public IActionResult DeleteRange([FromBody] DeleteRangeQuestionRepoRequest request)
    {
        var result = _questionRepoService.DeleteRange(request);
        return result ? NoContent() : BadRequest();
    }
} 
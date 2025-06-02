using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.QuestionRepo;
using QuizApp.Application.DTOs.Responses.QuestionRepo;
using QuizApp.Application.Services;
using QuizApp.Domain.Constants;

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

    [HttpGet("{questionRepoId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public async Task<IActionResult> GetQuestionRepoById([FromRoute] Guid questionRepoId)
    {
        var result = await _questionRepoService.GetByIdAsync(new GetQuestionRepoByIdRequest { Id = questionRepoId });
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public IActionResult GetAllQuestionRepos([FromQuery] GetQuestionReposRequest request)
    {
        var result = _questionRepoService.GetAll(request);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public async Task<IActionResult> CreateQuestionRepo([FromBody] CreateQuestionRepoRequest request)
    {
        var result = await _questionRepoService.CreateAsync(request);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public IActionResult UpdateQuestionRepo([FromBody] UpdateQuestionRepoRequest request)
    {
        var result = _questionRepoService.Update(request);
        return Ok(result);
    }

    [HttpDelete]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public async Task<IActionResult> DeleteQuestionRepo([FromBody] DeleteQuestionRepoRequest request)
    {
        await _questionRepoService.DeleteAsync(request);
        return Ok();
    }

    [HttpDelete("range")]
    public IActionResult DeleteRange([FromBody] DeleteRangeQuestionRepoRequest request)
    {
        var result = _questionRepoService.DeleteRange(request);
        return Ok(result);
    }
} 
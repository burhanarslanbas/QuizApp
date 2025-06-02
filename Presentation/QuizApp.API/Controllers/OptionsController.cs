using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Option;
using QuizApp.Application.DTOs.Responses.Option;
using QuizApp.Application.Services;
using System.Security.Claims;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OptionsController : ControllerBase
{
    private readonly IOptionService _optionService;

    public OptionsController(IOptionService optionService)
    {
        _optionService = optionService;
    }

    /// <summary>
    /// Id ile seçenek getirir
    /// </summary>
    [HttpGet("{optionId}")]
    public async Task<IActionResult> GetOptionById([FromRoute] Guid optionId)
    {
        var result = await _optionService.GetByIdAsync(optionId);
        return Ok(result);
    }

    /// <summary>
    /// Tüm seçenekleri getirir
    /// </summary>
    [HttpGet]
    public IActionResult GetAllOptions()
    {
        var result = _optionService.GetAll();
        return Ok(result);
    }

    /// <summary>
    /// Yeni seçenek oluşturur
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> CreateOption([FromBody] CreateOptionRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _optionService.CreateAsync(request, Guid.Parse(userId));
        return Ok(result);
    }

    /// <summary>
    /// Çoklu seçenek oluşturur
    /// </summary>
    [HttpPost("range")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> CreateRange([FromBody] List<CreateOptionRequest> requests)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _optionService.CreateRangeAsync(requests, Guid.Parse(userId));
        return Ok(result);
    }

    /// <summary>
    /// Seçenek günceller
    /// </summary>
    [HttpPut]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> UpdateOption([FromBody] UpdateOptionRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _optionService.Update(request, Guid.Parse(userId));
        return Ok(result);
    }

    /// <summary>
    /// Seçenek siler
    /// </summary>
    [HttpDelete("{optionId}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> DeleteOption([FromRoute] Guid optionId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _optionService.DeleteAsync(optionId, Guid.Parse(userId));
        return Ok(result);
    }

    /// <summary>
    /// Çoklu seçenek siler
    /// </summary>
    [HttpDelete("range")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> DeleteRange([FromBody] List<Guid> ids)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _optionService.DeleteRange(ids, Guid.Parse(userId));
        return Ok(result);
    }
} 
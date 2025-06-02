using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Category;
using QuizApp.Application.Services;
using QuizApp.Domain.Constants;

namespace QuizApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("{categoryId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid categoryId)
    {
        var result = await _categoryService.GetByIdAsync(new GetCategoryByIdRequest { Id = categoryId });
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    public IActionResult GetAllCategories([FromQuery] GetCategoriesRequest request)
    {
        var result = _categoryService.GetAll(request);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
    {
        var result = await _categoryService.CreateAsync(request);
        return Ok(result);
    }

    [HttpPost("range")]
    public async Task<IActionResult> CreateRange([FromBody] CreateRangeCategoryRequest request)
    {
        var result = await _categoryService.CreateRange(request);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public IActionResult UpdateCategory([FromBody] UpdateCategoryRequest request)
    {
        var result = _categoryService.Update(request);
        return Ok(result);
    }

    [HttpPut("range")]
    public IActionResult UpdateRange([FromBody] UpdateRangeCategoryRequest request)
    {
        var result = _categoryService.UpdateRange(request);
        return Ok(result);
    }

    [HttpDelete]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    public async Task<IActionResult> DeleteCategory([FromBody] DeleteCategoryRequest request)
    {
        await _categoryService.DeleteAsync(request);
        return Ok();
    }

    [HttpDelete("range")]
    public IActionResult DeleteRange([FromBody] DeleteRangeCategoryRequest request)
    {
        var result = _categoryService.DeleteRange(request);
        return Ok(result);
    }
}
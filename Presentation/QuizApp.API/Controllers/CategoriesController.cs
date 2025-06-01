using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Category;
using QuizApp.Application.DTOs.Responses.Category;
using QuizApp.Application.Services;

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

    /// <summary>
    /// Id ile kategori getirir
    /// </summary>
    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid categoryId)
    {
        var result = await _categoryService.GetByIdAsync(new GetCategoryByIdRequest { Id = categoryId });
        return Ok(result);
    }

    /// <summary>
    /// Tüm kategorileri getirir
    /// </summary>
    [HttpGet]
    public IActionResult GetAllCategories([FromQuery] GetCategoriesRequest request)
    {
        var result = _categoryService.GetAll(request);
        return Ok(result);
    }

    /// <summary>
    /// Yeni kategori oluşturur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
    {
        var result = await _categoryService.CreateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Çoklu kategori oluşturur
    /// </summary>
    [HttpPost("range")]
    public async Task<IActionResult> CreateRange([FromBody] CreateRangeCategoryRequest request)
    {
        var result = await _categoryService.CreateRange(request);
        return Ok(result);
    }

    /// <summary>
    /// Kategori günceller
    /// </summary>
    [HttpPut]
    public IActionResult UpdateCategory([FromBody] UpdateCategoryRequest request)
    {
        var result = _categoryService.Update(request);
        return Ok(result);
    }

    /// <summary>
    /// Çoklu kategori günceller
    /// </summary>
    [HttpPut("range")]
    public IActionResult UpdateRange([FromBody] UpdateRangeCategoryRequest request)
    {
        var result = _categoryService.UpdateRange(request);
        return Ok(result);
    }

    /// <summary>
    /// Kategori siler
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> DeleteCategory([FromBody] DeleteCategoryRequest request)
    {
        await _categoryService.DeleteAsync(request);
        return Ok();
    }

    /// <summary>
    /// Çoklu kategori siler
    /// </summary>
    [HttpDelete("range")]
    public IActionResult DeleteRange([FromBody] DeleteRangeCategoryRequest request)
    {
        var result = _categoryService.DeleteRange(request);
        return Ok(result);
    }
} 
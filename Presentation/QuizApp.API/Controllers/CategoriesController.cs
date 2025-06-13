using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Category;
using QuizApp.Application.DTOs.Responses.Category;
using QuizApp.Application.Services;
using QuizApp.Domain.Constants;

namespace QuizApp.API.Controllers;

[Route("api/categories")]
[ApiController]
[Authorize]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Get a category by ID
    /// </summary>
    /// <param name="categoryId">Category ID</param>
    /// <returns>Category details</returns>
    [HttpGet("{categoryId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid categoryId)
    {
        var result = await _categoryService.GetByIdAsync(new GetCategoryByIdRequest { Id = categoryId });
        return Ok(result);
    }

    /// <summary>
    /// Get all categories with pagination
    /// </summary>
    /// <param name="request">Pagination parameters</param>
    /// <returns>List of categories</returns>
    [HttpGet]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher},{RoleConstants.Roles.Student}")]
    [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllCategories([FromQuery] GetCategoriesRequest request)
    {
        var result = await _categoryService.GetAllAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Create a new category
    /// </summary>
    /// <param name="request">Category information</param>
    /// <returns>Created category</returns>
    [HttpPost]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
    {
        var result = await _categoryService.CreateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Update an existing category
    /// </summary>
    /// <param name="request">Updated category information</param>
    /// <returns>Updated category</returns>
    [HttpPut]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest request)
    {
        var result = await _categoryService.UpdateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Delete a category
    /// </summary>
    /// <param name="categoryId">Category ID to delete</param>
    /// <returns>No content</returns>
    [HttpDelete("{categoryId}")]
    [Authorize(Roles = $"{RoleConstants.Roles.Admin},{RoleConstants.Roles.Teacher}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid categoryId)
    {
        await _categoryService.DeleteAsync(new DeleteCategoryRequest(categoryId));
        return NoContent();
    }
}
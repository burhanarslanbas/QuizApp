using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizApp.Application.DTOs.Requests.Category;
using QuizApp.Application.DTOs.Responses.Category;
using QuizApp.Application.Exceptions;
using QuizApp.Application.Repositories.Category;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;

namespace QuizApp.Infrastructure.Managers;

public class CategoryManager : ICategoryService
{
    private readonly ICategoryReadRepository _categoryReadRepository;
    private readonly ICategoryWriteRepository _categoryWriteRepository;
    private readonly IMapper _mapper;

    public CategoryManager(ICategoryReadRepository categoryReadRepository, ICategoryWriteRepository categoryWriteRepository, IMapper mapper)
    {
        _categoryReadRepository = categoryReadRepository;
        _categoryWriteRepository = categoryWriteRepository;
        _mapper = mapper;
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request)
    {
        var category = _mapper.Map<Category>(request);
        await _categoryWriteRepository.AddAsync(category);
        await _categoryWriteRepository.SaveAsync();
        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task<CategoryResponse> UpdateAsync(UpdateCategoryRequest request)
    {
        var category = await _categoryReadRepository.GetByIdAsync(request.Id);
        if (category == null)
            throw new NotFoundException($"Category with ID {request.Id} not found");

        _mapper.Map(request, category);
        _categoryWriteRepository.Update(category);
        await _categoryWriteRepository.SaveAsync();
        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task DeleteAsync(DeleteCategoryRequest request)
    {
        var category = await _categoryReadRepository.GetByIdAsync(request.Id);
        if (category == null)
            throw new NotFoundException($"Category with ID {request.Id} not found");

        _categoryWriteRepository.Remove(category);
        await _categoryWriteRepository.SaveAsync();
    }

    public async Task<CategoryResponse> GetByIdAsync(GetCategoryByIdRequest request)
    {
        var category = await _categoryReadRepository.GetAll()
            .Include(c => c.Quizzes)
            .FirstOrDefaultAsync(c => c.Id == request.Id);

        if (category == null)
            throw new NotFoundException($"Category with ID {request.Id} not found");

        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task<IEnumerable<CategoryResponse>> GetAllAsync(GetCategoriesRequest request)
    {
        var query = _categoryReadRepository.GetAll()
            .Include(c => c.Quizzes)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.SearchText))
            query = query.Where(c => c.Name.Contains(request.SearchText));

        if (request.ParentCategoryId != Guid.Empty)
            query = query.Where(c => c.ParentCategoryId == request.ParentCategoryId);

        query = query.Where(c => c.IsActive == request.IsActive);

        var categories = await query.ToListAsync();
        return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
    }

    public async Task<IEnumerable<CategoryResponse>> CreateRangeAsync(CreateRangeCategoryRequest request)
    {
        var categories = _mapper.Map<IEnumerable<Category>>(request.Categories);
        await _categoryWriteRepository.AddRangeAsync(categories.ToList());
        await _categoryWriteRepository.SaveAsync();
        return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
    }

    public async Task<IEnumerable<CategoryResponse>> UpdateRangeAsync(UpdateRangeCategoryRequest request)
    {
        var categories = await _categoryReadRepository.GetAll()
            .Where(c => request.Ids.Contains(c.Id))
            .ToListAsync();

        if (categories.Count != request.Ids.Count)
            throw new NotFoundException("One or more categories not found");

        foreach (var category in categories)
        {
            var updateRequest = request.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (updateRequest != null)
                _mapper.Map(updateRequest, category);
        }

        _categoryWriteRepository.UpdateRange(categories.ToList());
        await _categoryWriteRepository.SaveAsync();
        return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
    }

    public async Task DeleteRangeAsync(DeleteRangeCategoryRequest request)
    {
        var categories = await _categoryReadRepository.GetAll()
            .Where(c => request.Ids.Contains(c.Id))
            .ToListAsync();

        if (categories.Count != request.Ids.Count)
            throw new NotFoundException("One or more categories not found");

        _categoryWriteRepository.RemoveRange(categories);
        await _categoryWriteRepository.SaveAsync();
    }
}
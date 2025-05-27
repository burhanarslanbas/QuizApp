using AutoMapper;
using QuizApp.Application.DTOs.Requests.Category;
using QuizApp.Application.DTOs.Responses.Category;
using QuizApp.Application.Repositories;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;
using QuizApp.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

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

    public async Task<CategoryDetailResponse> CreateAsync(CreateCategoryRequest request)
    {
        var category = _mapper.Map<Category>(request);
        var result = await _categoryWriteRepository.AddAsync(category);
        if (!result)
            throw new BusinessException("Failed to create category");

        return _mapper.Map<CategoryDetailResponse>(category);
    }

    public async Task<List<CategoryDetailResponse>> CreateRange(CreateRangeCategoryRequest request)
    {
        var categories = _mapper.Map<List<Category>>(request.Categories);
        var result = await _categoryWriteRepository.AddRangeAsync(categories);
        if (!result)
            throw new BusinessException("Failed to create categories");

        return _mapper.Map<List<CategoryDetailResponse>>(categories);
    }

    public async Task DeleteAsync(DeleteCategoryRequest request)
    {
        var result = await _categoryWriteRepository.RemoveById(request.Id);
        if (!result)
            throw new NotFoundException($"Category with ID {request.Id} not found.");
    }

    public bool DeleteRange(DeleteRangeCategoryRequest request)
    {
        var categories = _categoryReadRepository.GetWhere(x => request.Ids.Contains(x.Id)).ToList();
        if (!categories.Any())
            throw new NotFoundException("No categories found with the provided IDs.");

        var result = _categoryWriteRepository.RemoveRange(categories);
        if (!result)
            throw new BusinessException("Failed to delete categories.");

        return result;
    }

    public List<CategoryDetailResponse> GetAll(GetCategoriesRequest request)
    {
        var categories = _categoryReadRepository.GetAll().ToList();
        return _mapper.Map<List<CategoryDetailResponse>>(categories);
    }

    public async Task<CategoryDetailResponse> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var category = await _categoryReadRepository.GetByIdAsync(request.Id);
            return _mapper.Map<CategoryDetailResponse>(category);
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException($"Category with ID {request.Id} not found.");
        }
    }

    public CategoryDetailResponse Update(UpdateCategoryRequest request)
    {
        try
        {
            var category = _categoryReadRepository.GetByIdAsync(request.Id).Result;
            _mapper.Map(request, category);
            var result = _categoryWriteRepository.Update(category);
            if (!result)
                throw new BusinessException($"Failed to update category with ID {request.Id}");

            return _mapper.Map<CategoryDetailResponse>(category);
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException($"Category with ID {request.Id} not found.");
        }
    }

    public List<CategoryDetailResponse> UpdateRange(UpdateRangeCategoryRequest request)
    {
        var categories = _categoryReadRepository.GetWhere(x => request.Categories.Select(c => c.Id).Contains(x.Id)).ToList();
        if (!categories.Any())
            throw new NotFoundException("No categories found with the provided IDs.");

        foreach (var category in categories)
        {
            var updateRequest = request.Categories.First(c => c.Id == category.Id);
            _mapper.Map(updateRequest, category);
            var result = _categoryWriteRepository.Update(category);
            if (!result)
                throw new BusinessException($"Failed to update category with ID {category.Id}");
        }

        return _mapper.Map<List<CategoryDetailResponse>>(categories);
    }
}
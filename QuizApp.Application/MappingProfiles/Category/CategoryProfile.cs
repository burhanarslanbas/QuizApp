using AutoMapper;
using QuizApp.Application.DTOs.Requests.Category;
using QuizApp.Application.DTOs.Responses.Category;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.MappingProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<Category, CreateCategoryRequest>().ReverseMap();
        CreateMap<Category, CreateRangeCategoryRequest>().ReverseMap();
        CreateMap<Category, DeleteCategoryRequest>().ReverseMap();
        CreateMap<Category, DeleteRangeCategoryRequest>().ReverseMap();
        CreateMap<Category, UpdateCategoryRequest>().ReverseMap();
        CreateMap<Category, UpdateRangeCategoryRequest>().ReverseMap();
    }
}

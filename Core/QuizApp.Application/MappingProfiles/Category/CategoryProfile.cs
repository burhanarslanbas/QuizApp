using AutoMapper;
using QuizApp.Application.DTOs.Requests.Category;
using QuizApp.Application.DTOs.Responses.Category;

namespace QuizApp.Application.MappingProfiles.Category;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        // Request to Entity
        CreateMap<CreateCategoryRequest, Domain.Entities.Category>();
        CreateMap<UpdateCategoryRequest, Domain.Entities.Category>();
        CreateMap<DeleteCategoryRequest, Domain.Entities.Category>();
        CreateMap<GetCategoryByIdRequest, Domain.Entities.Category>();
        CreateMap<GetCategoriesRequest, Domain.Entities.Category>();
        
        // Bulk Operations
        CreateMap<CreateRangeCategoryRequest, Domain.Entities.Category>();
        CreateMap<UpdateRangeCategoryRequest, Domain.Entities.Category>();
        CreateMap<DeleteRangeCategoryRequest, Domain.Entities.Category>();

        // Entity to Response
        CreateMap<Domain.Entities.Category, CategoryResponse>();
    }
}

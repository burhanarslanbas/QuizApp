using AutoMapper;
using QuizApp.Application.DTOs.Requests.Quiz;
using QuizApp.Application.DTOs.Responses.Quiz;

namespace QuizApp.Application.MappingProfiles.Quiz;

public class QuizProfile : Profile
{
    public QuizProfile()
    {
        // Request to Entity
        CreateMap<CreateQuizRequest, Domain.Entities.Quiz>();
        CreateMap<UpdateQuizRequest, Domain.Entities.Quiz>();
        CreateMap<DeleteQuizRequest, Domain.Entities.Quiz>();
        CreateMap<GetQuizByIdRequest, Domain.Entities.Quiz>();
        CreateMap<GetQuizzesRequest, Domain.Entities.Quiz>();
        CreateMap<GetQuizzesByCategoryRequest, Domain.Entities.Quiz>();
        CreateMap<GetQuizzesByUserRequest, Domain.Entities.Quiz>();
        CreateMap<GetActiveQuizzesRequest, Domain.Entities.Quiz>();

        // Bulk Operations
        CreateMap<CreateRangeQuizRequest, Domain.Entities.Quiz>();
        CreateMap<UpdateRangeQuizRequest, Domain.Entities.Quiz>();
        CreateMap<DeleteRangeQuizRequest, Domain.Entities.Quiz>();

        // Entity to Response
        CreateMap<Domain.Entities.Quiz, QuizResponse>();
    }
}
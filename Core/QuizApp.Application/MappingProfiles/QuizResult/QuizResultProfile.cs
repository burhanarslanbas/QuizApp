using AutoMapper;
using QuizApp.Application.DTOs.Requests.QuizResult;
using QuizApp.Application.DTOs.Responses.QuizResult;

namespace QuizApp.Application.MappingProfiles.QuizResult;

public class QuizResultProfile : Profile
{
    public QuizResultProfile()
    {
        // Request to Entity
        CreateMap<CreateQuizResultRequest, Domain.Entities.QuizResult>();
        CreateMap<UpdateQuizResultRequest, Domain.Entities.QuizResult>();
        CreateMap<DeleteQuizResultRequest, Domain.Entities.QuizResult>();
        CreateMap<GetQuizResultByIdRequest, Domain.Entities.QuizResult>();
        CreateMap<GetQuizResultsRequest, Domain.Entities.QuizResult>();
        CreateMap<GetQuizResultsByUserRequest, Domain.Entities.QuizResult>();
        CreateMap<GetQuizResultsByQuizRequest, Domain.Entities.QuizResult>();

        // Bulk Operations
        CreateMap<CreateRangeQuizResultRequest, Domain.Entities.QuizResult>();
        CreateMap<UpdateRangeQuizResultRequest, Domain.Entities.QuizResult>();
        CreateMap<DeleteRangeQuizResultRequest, Domain.Entities.QuizResult>();

        // Entity to Response
        CreateMap<Domain.Entities.QuizResult, QuizResultResponse>();
    }
}
using AutoMapper;
using QuizApp.Application.DTOs.Requests.Question;
using QuizApp.Application.DTOs.Responses.Question;

namespace QuizApp.Application.MappingProfiles.Question;

public class QuestionProfile : Profile
{
    public QuestionProfile()
    {
        // Request to Entity
        CreateMap<CreateQuestionRequest, Domain.Entities.Question>();
        CreateMap<UpdateQuestionRequest, Domain.Entities.Question>();
        CreateMap<DeleteQuestionRequest, Domain.Entities.Question>();
        CreateMap<GetQuestionByIdRequest, Domain.Entities.Question>();
        CreateMap<GetQuestionsRequest, Domain.Entities.Question>();
        CreateMap<GetQuestionsByRepoRequest, Domain.Entities.Question>();

        // Bulk Operations
        CreateMap<CreateRangeQuestionRequest, Domain.Entities.Question>();
        CreateMap<UpdateRangeQuestionRequest, Domain.Entities.Question>();
        CreateMap<DeleteRangeQuestionRequest, Domain.Entities.Question>();
        CreateMap<UpdateQuestionRepoIdRequest, Domain.Entities.Question>();
        CreateMap<UpdateQuestionRepoIdsRequest, Domain.Entities.Question>();

        // Entity to Response
        CreateMap<Domain.Entities.Question, QuestionResponse>();
    }
}
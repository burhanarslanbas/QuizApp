using AutoMapper;
using QuizApp.Application.DTOs.Requests.QuestionRepo;
using QuizApp.Application.DTOs.Responses.QuestionRepo;

namespace QuizApp.Application.MappingProfiles.QuestionRepo;

public class QuestionRepoProfile : Profile
{
    public QuestionRepoProfile()
    {
        // Request to Entity
        CreateMap<CreateQuestionRepoRequest, Domain.Entities.QuestionRepo>();
        CreateMap<UpdateQuestionRepoRequest, Domain.Entities.QuestionRepo>();
        CreateMap<DeleteQuestionRepoRequest, Domain.Entities.QuestionRepo>();
        CreateMap<GetQuestionRepoByIdRequest, Domain.Entities.QuestionRepo>();
        CreateMap<GetQuestionReposRequest, Domain.Entities.QuestionRepo>();
        CreateMap<GetQuestionReposByUserRequest, Domain.Entities.QuestionRepo>();

        // Bulk Operations
        CreateMap<DeleteRangeQuestionRepoRequest, Domain.Entities.QuestionRepo>();

        // Entity to Response
        CreateMap<Domain.Entities.QuestionRepo, QuestionRepoResponse>();
    }
}
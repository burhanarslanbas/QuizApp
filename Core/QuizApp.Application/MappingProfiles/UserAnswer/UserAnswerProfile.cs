using AutoMapper;
using QuizApp.Application.DTOs.Requests.UserAnswer;
using QuizApp.Application.DTOs.Responses.UserAnswer;

namespace QuizApp.Application.MappingProfiles.UserAnswer;

public class UserAnswerProfile : Profile
{
    public UserAnswerProfile()
    {
        // Request to Entity
        CreateMap<CreateUserAnswerRequest, Domain.Entities.UserAnswer>();
        CreateMap<UpdateUserAnswerRequest, Domain.Entities.UserAnswer>();
        CreateMap<DeleteUserAnswerRequest, Domain.Entities.UserAnswer>();
        CreateMap<GetUserAnswerByIdRequest, Domain.Entities.UserAnswer>();
        CreateMap<GetUserAnswersRequest, Domain.Entities.UserAnswer>();
        CreateMap<GetUserAnswersByQuizResultRequest, Domain.Entities.UserAnswer>();

        // Bulk Operations
        CreateMap<DeleteRangeUserAnswerRequest, Domain.Entities.UserAnswer>();

        // Entity to Response
        CreateMap<Domain.Entities.UserAnswer, UserAnswerResponse>();
    }
}
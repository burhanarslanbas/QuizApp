using AutoMapper;
using QuizApp.Application.DTOs.Requests.Option;
using QuizApp.Application.DTOs.Responses.Option;

namespace QuizApp.Application.MappingProfiles.Option;

public class OptionProfile : Profile
{
    public OptionProfile()
    {
        // Request to Entity
        CreateMap<CreateOptionRequest, Domain.Entities.Option>();
        CreateMap<UpdateOptionRequest, Domain.Entities.Option>();
        CreateMap<DeleteOptionRequest, Domain.Entities.Option>();
        CreateMap<GetOptionByIdRequest, Domain.Entities.Option>();
        CreateMap<GetOptionsByQuestionRequest, Domain.Entities.Option>();

        // Entity to Response
        CreateMap<Domain.Entities.Option, OptionResponse>();
    }
}
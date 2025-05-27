using AutoMapper;
using QuizApp.Application.DTOs.Requests.Option;
using QuizApp.Application.DTOs.Responses.Option;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.MappingProfiles;

public class OptionProfile : Profile
{
    public OptionProfile()
    {
        CreateMap<Option, OptionDTO>().ReverseMap();
        CreateMap<Option, CreateOptionRequest>().ReverseMap();
        CreateMap<Option, UpdateOptionRequest>().ReverseMap();
        CreateMap<Option, DeleteOptionRequest>().ReverseMap();
        CreateMap<Option, GetOptionByIdRequest>().ReverseMap();
        CreateMap<Option, GetOptionsByQuestionRequest>().ReverseMap();
    }
}
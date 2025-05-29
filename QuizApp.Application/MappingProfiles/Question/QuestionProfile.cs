using AutoMapper;
using QuizApp.Application.DTOs.Requests.Question;
using QuizApp.Application.DTOs.Responses.Question;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.MappingProfiles;

public class QuestionProfile : Profile
{
    public QuestionProfile()
    {
        // Basic CRUD mappings
        CreateMap<Question, QuestionDTO>().ReverseMap();
        CreateMap<Question, CreateQuestionRequest>().ReverseMap();
        CreateMap<Question, UpdateQuestionRequest>().ReverseMap();
        CreateMap<Question, DeleteQuestionRequest>().ReverseMap();
        CreateMap<Question, GetQuestionByIdRequest>().ReverseMap();

        // Detail response mapping
        CreateMap<Question, QuestionDetailResponse>()
            .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options));
    }
}
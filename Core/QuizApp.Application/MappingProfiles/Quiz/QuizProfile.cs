using AutoMapper;
using QuizApp.Application.DTOs.Requests.Quiz;
using QuizApp.Application.DTOs.Responses.Option;
using QuizApp.Application.DTOs.Responses.Question;
using QuizApp.Application.DTOs.Responses.Quiz;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.MappingProfiles;

public class QuizProfile : Profile
{
    public QuizProfile()
    {
        // Basic CRUD mappings
        CreateMap<Quiz, QuizDTO>().ReverseMap();
        CreateMap<Quiz, CreateQuizRequest>().ReverseMap();
        CreateMap<Quiz, UpdateQuizRequest>().ReverseMap();
        CreateMap<Quiz, DeleteQuizRequest>().ReverseMap();
        CreateMap<Quiz, GetQuizByIdRequest>().ReverseMap();

        // Detail response mapping
        CreateMap<Quiz, QuizDetailResponse>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

        // Related entity mappings
        CreateMap<Question, QuestionDetailResponse>()
            .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options));

        CreateMap<Option, OptionDetailResponse>();
    }
}
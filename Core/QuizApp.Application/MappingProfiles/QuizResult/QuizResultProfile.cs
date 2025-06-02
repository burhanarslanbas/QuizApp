using AutoMapper;
using QuizApp.Application.DTOs.Requests.QuizResult;
using QuizApp.Application.DTOs.Responses.QuizResult;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.MappingProfiles;

public class QuizResultProfile : Profile
{
    public QuizResultProfile()
    {
        // Basic CRUD mappings
        CreateMap<QuizResult, QuizResultDTO>().ReverseMap();
        CreateMap<QuizResult, CreateQuizResultRequest>().ReverseMap();
        CreateMap<QuizResult, UpdateQuizResultRequest>().ReverseMap();
        CreateMap<QuizResult, DeleteQuizResultRequest>().ReverseMap();
        CreateMap<QuizResult, GetQuizResultByIdRequest>().ReverseMap();

        // Get operations
        CreateMap<QuizResult, GetQuizResultsRequest>().ReverseMap();
        CreateMap<QuizResult, GetQuizResultsByUserRequest>().ReverseMap();
        CreateMap<QuizResult, GetQuizResultsByQuizRequest>().ReverseMap();

        // Detail response mapping
        CreateMap<QuizResult, QuizResultDetailResponse>()
            .ForMember(dest => dest.QuizTitle, opt => opt.MapFrom(src => src.Quiz.Title))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Student.UserName))
            .ForMember(dest => dest.UserAnswers, opt => opt.MapFrom(src => src.StudentAnswers));
    }
}
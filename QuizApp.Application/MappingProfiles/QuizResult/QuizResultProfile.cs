using AutoMapper;
using QuizApp.Application.DTOs.Requests.QuizResult;
using QuizApp.Application.DTOs.Responses.QuizResult;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.MappingProfiles;

public class QuizResultProfile : Profile
{
    public QuizResultProfile()
    {
        CreateMap<QuizResult, QuizResultDTO>().ReverseMap();
        CreateMap<QuizResult, CreateQuizResultRequest>().ReverseMap();
        CreateMap<QuizResult, UpdateQuizResultRequest>().ReverseMap();
        CreateMap<QuizResult, DeleteQuizResultRequest>().ReverseMap();
        CreateMap<QuizResult, GetQuizResultByIdRequest>().ReverseMap();
        CreateMap<QuizResult, GetQuizResultsByUserRequest>().ReverseMap();
        CreateMap<QuizResult, GetQuizResultsByQuizRequest>().ReverseMap();
    }
}
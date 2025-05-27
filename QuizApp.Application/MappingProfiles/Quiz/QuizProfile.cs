using AutoMapper;
using QuizApp.Application.DTOs.Requests.Quiz;
using QuizApp.Application.DTOs.Responses.Quiz;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.MappingProfiles;

public class QuizProfile : Profile
{
    public QuizProfile()
    {
        CreateMap<Quiz, QuizDTO>().ReverseMap();
        CreateMap<Quiz, CreateQuizRequest>().ReverseMap();
        CreateMap<Quiz, UpdateQuizRequest>().ReverseMap();
        CreateMap<Quiz, DeleteQuizRequest>().ReverseMap();
        CreateMap<Quiz, GetQuizByIdRequest>().ReverseMap();
        CreateMap<Quiz, GetQuizzesByCategoryRequest>().ReverseMap();
        CreateMap<Quiz, GetActiveQuizzesRequest>().ReverseMap();
    }
}
using AutoMapper;
using QuizApp.Application.DTOs.Requests.QuestionRepo;
using QuizApp.Application.DTOs.Responses.QuestionRepo;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.MappingProfiles;

public class QuestionRepoProfile : Profile
{
    public QuestionRepoProfile()
    {
        CreateMap<QuestionRepo, QuestionRepoDTO>().ReverseMap();
        CreateMap<QuestionRepo, CreateQuestionRepoRequest>().ReverseMap();
        CreateMap<QuestionRepo, UpdateQuestionRepoRequest>().ReverseMap();
        CreateMap<QuestionRepo, DeleteQuestionRepoRequest>().ReverseMap();
        CreateMap<QuestionRepo, GetQuestionRepoByIdRequest>().ReverseMap();
        CreateMap<QuestionRepo, GetQuestionReposByCategoryRequest>().ReverseMap();
    }
}
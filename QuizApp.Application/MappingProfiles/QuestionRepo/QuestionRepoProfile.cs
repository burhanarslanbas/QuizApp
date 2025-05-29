using AutoMapper;
using QuizApp.Application.DTOs.Requests.QuestionRepo;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.MappingProfiles;

public class QuestionRepoProfile : Profile
{
    public QuestionRepoProfile()
    {
        CreateMap<QuestionRepo, CreateQuestionRepoRequest>().ReverseMap();
    }
}
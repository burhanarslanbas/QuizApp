using AutoMapper;
using QuizApp.Application.DTOs.Requests.UserAnswer;
using QuizApp.Application.DTOs.Responses.UserAnswer;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.MappingProfiles;

public class UserAnswerProfile : Profile
{
    public UserAnswerProfile()
    {
        CreateMap<UserAnswer, UserAnswerDTO>().ReverseMap();
        CreateMap<UserAnswer, CreateUserAnswerRequest>().ReverseMap();
        CreateMap<UserAnswer, UpdateUserAnswerRequest>().ReverseMap();
        CreateMap<UserAnswer, DeleteUserAnswerRequest>().ReverseMap();
        CreateMap<UserAnswer, GetUserAnswerByIdRequest>().ReverseMap();
        CreateMap<UserAnswer, GetUserAnswersByQuizResultRequest>().ReverseMap();
    }
}
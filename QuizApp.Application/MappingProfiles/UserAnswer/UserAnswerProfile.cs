using AutoMapper;
using QuizApp.Application.DTOs.Requests.UserAnswer;
using QuizApp.Application.DTOs.Responses.UserAnswer;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.MappingProfiles;

public class UserAnswerProfile : Profile
{
    public UserAnswerProfile()
    {
        // Basic CRUD mappings
        CreateMap<UserAnswer, UserAnswerDTO>().ReverseMap();
        CreateMap<UserAnswer, CreateUserAnswerRequest>().ReverseMap();
        CreateMap<UserAnswer, UpdateUserAnswerRequest>().ReverseMap();
        CreateMap<UserAnswer, DeleteUserAnswerRequest>().ReverseMap();
        CreateMap<UserAnswer, GetUserAnswerByIdRequest>().ReverseMap();

        // Detail response mapping
        CreateMap<UserAnswer, UserAnswerDetailResponse>()
            .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Question != null ? src.Question.QuestionText : null))
            .ForMember(dest => dest.SelectedOptionText, opt => opt.MapFrom(src => src.Option != null ? src.Option.OptionText : null))
            .ForMember(dest => dest.CorrectOptionText, opt => opt.MapFrom(src => 
                src.Question != null && src.Question.Options != null ? 
                (src.Question.Options.FirstOrDefault(o => o.IsCorrect) != null ? 
                    src.Question.Options.FirstOrDefault(o => o.IsCorrect).OptionText : null) : null));
    }
}
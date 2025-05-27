using AutoMapper;
using QuizApp.Application.DTOs.Requests.User;
using QuizApp.Application.DTOs.Responses.Quiz;
using QuizApp.Application.DTOs.Responses.User;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<User, CreateUserRequest>().ReverseMap();
        CreateMap<User, UpdateUserRequest>().ReverseMap();
        CreateMap<User, DeleteUserRequest>().ReverseMap();
        CreateMap<User, GetUserByIdRequest>().ReverseMap();
        CreateMap<User, GetUserByEmailRequest>().ReverseMap();
        CreateMap<User, GetUserByUsernameRequest>().ReverseMap();
        CreateMap<User, DeleteRangeUserRequest>().ReverseMap();
        CreateMap<User, GetUsersRequest>().ReverseMap();

        CreateMap<User, UserDetailResponse>()
            .ForMember(dest => dest.RecentQuizResults, opt => opt.MapFrom(src => src.QuizResults.OrderByDescending(qr => qr.CreatedDate).Take(5)));

        CreateMap<QuizResult, QuizResultSummaryResponse>()
            .ForMember(dest => dest.QuizTitle, opt => opt.MapFrom(src => src.Quiz.Title))
            .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => src.CreatedDate));
    }
}
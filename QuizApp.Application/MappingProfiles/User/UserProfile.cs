using AutoMapper;
using QuizApp.Application.DTOs.Requests.User;
using QuizApp.Application.DTOs.Responses.User;
using QuizApp.Domain.Entities;

namespace QuizApp.Application.MappingProfiles.User;

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
    }
} 
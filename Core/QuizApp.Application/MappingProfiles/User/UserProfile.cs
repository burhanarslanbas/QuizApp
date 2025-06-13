using AutoMapper;
using QuizApp.Application.DTOs.Responses.User;
using QuizApp.Domain.Entities.Identity;

namespace QuizApp.Application.MappingProfiles.User;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AppUser, UserResponse>();
    }
}
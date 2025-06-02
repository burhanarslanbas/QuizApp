using AutoMapper;
using QuizApp.Application.DTOs.Requests.Auth;
using QuizApp.Application.DTOs.Responses.Auth;
using QuizApp.Application.DTOs.Responses.Token;
using QuizApp.Domain.Entities.Identity;

namespace QuizApp.Application.MappingProfiles.Auth
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<AppUser, RegisterRequest>().ReverseMap();
            CreateMap<AppUser, LoginRequest>().ReverseMap();
            CreateMap<AppUser, RefreshTokenRequest>().ReverseMap();
            CreateMap<AppUser, RevokeTokenRequest>().ReverseMap();

            CreateMap<AppUser, Token>()
                .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
                .ForMember(dest => dest.Expiration, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshTokenExpiration, opt => opt.Ignore());
        }
    }
} 
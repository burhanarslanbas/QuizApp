using AutoMapper;
using QuizApp.Application.DTOs.Requests.Auth;
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

            /*CreateMap<AppUser, RegisterSuccessResponse>()
    .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => new UserInfo
    {
        Id = src.Id,
        UserName = src.UserName!,
        Email = src.Email!,
        FullName = src.FullName,
        PhoneNumber = src.PhoneNumber!,
        Roles = new List<string>()
    }));
*/
            /*CreateMap<AppUser, LoginSuccessResponse>()
            //    .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => new UserInfo
            //    {
            //        Id = src.Id,
            //        UserName = src.UserName!,
            //        Email = src.Email!,
            //        FullName = src.FullName,
            //        PhoneNumber = src.PhoneNumber!,
            //        Roles = new List<string>()
            //    }));
            */
        }
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using QuizApp.Application.DTOs.Requests.Auth;
using QuizApp.Application.DTOs.Responses.Auth;
using QuizApp.Application.DTOs.Responses.Token;
using QuizApp.Application.Services;
using QuizApp.Application.Services.Token;
using QuizApp.Domain.Entities.Identity;

namespace QuizApp.Infrastructure.Managers
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly IMapper _mapper;

        public AuthManager(UserManager<AppUser> userManager, IMapper mapper, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            // Kullanıcı adını veya e-posta adresini kullanarak kullanıcıyı bul
            var user = _userManager.Users.FirstOrDefault(u => u.UserName == request.UserNameOrEmail || u.Email == request.UserNameOrEmail);
            if (user == null)
            {
                return new LoginErrorResponse
                {
                    ErrorMessage = "Kullanıcı bulunamadı. Lütfen kullanıcı adınızı veya e-posta adresinizi kontrol edin.",
                    StatusCode = 404
                };
            }

            // Kullanıcıyı doğrula
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return new LoginErrorResponse
                {
                    ErrorMessage = "Giriş başarısız. Lütfen kullanıcı adınızı ve şifrenizi kontrol edin.",
                    StatusCode = 401
                };
            }

            // Giriş başarılı - Token oluştur
            Token token = _tokenHandler.CreateAccessToken(60, user); // 60 dakikalık token

            return new LoginSuccessResponse
            {
                Token = token,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user = _mapper.Map<AppUser>(request);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new RegisterResponse
                {
                    Succeeded = false,
                    Message = string.Join(", ", result.Errors.Select(x => x.Description))
                };
            }

            // Kullanıcı başarıyla oluşturuldu
            return new RegisterResponse
            {
                Succeeded = true,
                Message = "Kullanıcı başarıyla oluşturuldu.",
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }
}
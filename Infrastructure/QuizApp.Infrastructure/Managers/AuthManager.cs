using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using QuizApp.Application.DTOs.Requests.Auth;
using QuizApp.Application.DTOs.Responses.Auth;
using QuizApp.Application.Services;
using QuizApp.Application.Services.Token;
using QuizApp.Domain.Constants;
using QuizApp.Domain.Entities.Identity;

namespace QuizApp.Infrastructure.Managers;

public class AuthManager : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly Application.Options.TokenOptions _tokenOptions;
    private readonly IMapper _mapper;

    public AuthManager(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        IOptions<Application.Options.TokenOptions> tokenOptions,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _tokenOptions = tokenOptions.Value;
        _mapper = mapper;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        var user = new AppUser
        {
            UserName = request.UserName,
            Email = request.Email,
            FullName = request.FullName
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, RoleConstants.Roles.Student);

            return new RegisterResponse
            {
                Success = true,
                Message = "User registered successfully"
            };
        }

        return new RegisterErrorResponse
        {
            Success = false,
            Message = "Registration failed",
            Errors = result.Errors.Select(e => e.Description).ToList()
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new LoginErrorResponse
            {
                Success = false,
                Message = "Email not found",
                Errors = new List<string> { "Email not found" }
            };
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return new LoginErrorResponse
            {
                Success = false,
                Message = "Incorrect password",
                Errors = new List<string> { "Incorrect password" }
            };
        }

        var token = await _tokenService.CreateAccessTokenAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var claims = (await _userManager.GetClaimsAsync(user)).Select(c => c.Value).ToList();

        var userDto = _mapper.Map<Application.DTOs.Responses.User.UserResponse>(user);
        userDto.Roles = roles.ToList();
        userDto.Claims = claims;

        return new LoginSuccessResponse
        {
            Success = true,
            Message = "Login successful",
            Token = token,
            User = userDto
        };
    }

    public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        return await _tokenService.RefreshTokenAsync(request);
    }

    public async Task<LoginResponse> RevokeTokenAsync(RevokeTokenRequest request)
    {
        return await _tokenService.RevokeTokenAsync(request);
    }
}
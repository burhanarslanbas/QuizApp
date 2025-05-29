using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Requests.Auth;
using QuizApp.Application.DTOs.Responses.Auth;
using QuizApp.Application.Services;

namespace QuizApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            LoginResponse response = await _authService.LoginAsync(request);
            if (response is LoginErrorResponse errorResponse)
            {
                return StatusCode(errorResponse.StatusCode, errorResponse.ErrorMessage);
            }
            else if (response is LoginSuccessResponse successResponse)
            {
                return Ok(successResponse);
            }
            else
            {
                return StatusCode(500, "Unknown error occurred during login.");
            }
        }
    }
}
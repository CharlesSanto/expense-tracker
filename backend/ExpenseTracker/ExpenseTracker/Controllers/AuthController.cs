using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.DTOs.Auth;
using ExpenseTracker.Services.Interfaces;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _authService.LoginAsync(loginDto);

            if (user is null)
                return Unauthorized(new { Message = "Email ou senha incorretos." });

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                var newUser = await _userService.CreateUserAsync(registerDto);

                if (newUser is null)
                    return BadRequest(new { Message = "User registration failed" });

                return Created($"/api/users/{newUser.Id}", newUser);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { ex.Message });
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.DTOs.Auth;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            
            return Ok(new { Message = "Login successful" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            return Ok(new { Message = "Registration successful" });
        }
    }
}

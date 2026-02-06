using ExpenseTracker.DTOs.UserDTOs;
using ExpenseTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            try
            {
                var userId = GetUserId();
                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                    return NotFound(new { Message = "Usuário não encontrado." });

                return Ok(user);

            } catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto userDto)
        {
            try
            {
                var userId = GetUserId();
                var user = await _userService.UpdateUserAsync(userId, userDto);

                return Ok(user)
;
            } catch (InvalidOperationException ex)
            {
                return Conflict(new { ex.Message });
            }
        }

        [HttpDelete("me")]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                var userId = GetUserId();
                var deletedUser = await _userService.DeleteUserAsync(userId);

                if (!deletedUser)
                    return NotFound(new { Message = "Usuário não econtrado." });

                return NoContent();

            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}

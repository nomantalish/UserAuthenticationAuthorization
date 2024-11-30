using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserCreationAPI.DTOs;
using UserCreationAPI.Services.Contracts;

namespace UserCreationAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO input)
        {
            var token = await _userService.Login(input);
            if (token == null)
            {
                return NotFound();
            }

            return Ok(token);
        }
        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var token = await _userService.Refresh();
            return Ok(token); 
        }
        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO input)
        { 
            var result = await _userService.ChangePassword(input);
            return result? NoContent():BadRequest();
        }
        [Authorize(Roles = Constants.ROLE_ADMIN)]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO input)
        {
            var user = await _userService.Create(input);
            
            return user is null? Conflict() : Ok(user);
        }
    }
}

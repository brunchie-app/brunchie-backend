using brunchie_backend.DataBase;
using brunchie_backend.Models;
using brunchie_backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace brunchie_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;



        public AuthController(AuthService authService)
        {
            _authService = authService;

        }


        [HttpGet("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = _authService.Authenticate(loginDto);

                return Ok(new { token });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SignUp")]

        public async Task<IActionResult> SignUp([FromBody] SignUpDto signupdto)

        {
            


            try
            {
                var confirmation = await _authService.createUser(signupdto);
                return Ok(new { confirmation });


            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetInfo([FromQuery] string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return BadRequest("UserId empty");
            }

            try
            {
                var UserInfo = await _authService.UserInfo(UserId);
                return Ok(UserInfo);
            }

            catch (KeyNotFoundException ex)
            {

                return NotFound(new {ex.Message});
            }
        }

        
    }
}



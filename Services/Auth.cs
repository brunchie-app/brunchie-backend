using brunchie_backend.DataBase;
using brunchie_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace brunchie_backend.Services
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;

        }


        public async Task<String> Authenticate( LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null) throw new KeyNotFoundException("Invalid userId");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) throw new KeyNotFoundException("Invalid password.");


            var token = GenerateJwtToken(user);

            return token;
        }

        public async Task<string> createUser(SignUpDto signupdto)
        {
            User user = new User
            {
                UserName = signupdto.UserName,
                Email=signupdto.Email,
                CreatedAt=DateTime.UtcNow,
                CampusId=signupdto.CampusId,
            };


            try
            {
             var createResult = await _userManager.CreateAsync(user, signupdto.Password);
            if (!createResult.Succeeded)
             {
                        
              throw new Exception("User creation failed: " + string.Join(", ", createResult.Errors.Select(e => e.Description)));
             }

             var roleResult = await _userManager.AddToRoleAsync(user, signupdto.Role);
             if (!roleResult.Succeeded)
             {
              throw new Exception("Role assignment failed: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
             }

                return "User Creation Successful";  
            
            }
                catch (Exception ex)
                {
                    
                    throw;
                }
            }

        public async Task<UserResponseDto> UserInfo (string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                throw new KeyNotFoundException($"No user found with Id {UserId}");
            }

            return new UserResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                CreatedAt = user.CreatedAt

            };
           
                      
        }

        
        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(ClaimTypes.Name, user.UserName)
            
        };

            var roles = _userManager.GetRolesAsync(user).Result;
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tf2WeaponIndexAPI.Models;
using Tf2WeaponIndexAPI.Services;
using MongoDB.Driver;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Tf2WeaponIndexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;
        private readonly IConfiguration _configuration;

        public UserController(MongoDBService mongoDBService, IConfiguration configuration)
        {
            _mongoDBService = mongoDBService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User newUser)
        {

            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.PasswordHash);

            await _mongoDBService.CreateUser(newUser);
            return CreatedAtAction(nameof(Post), new { id = newUser.Id }, newUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginUser)
        {
            var user = await _mongoDBService.Users.Find(u => u.Username == loginUser.Username).FirstOrDefaultAsync();
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.PasswordHash, user.PasswordHash))
            {
                return Unauthorized("Invalid username or password.");
            }
            return Ok("Login successful.");
        }
        [HttpPost("loginJwt")]
        public async Task<IActionResult> LoginJwt([FromBody] User loginUser)
        {
            var user = await _mongoDBService.Users.Find(u => u.Username == loginUser.Username).FirstOrDefaultAsync();
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.PasswordHash, user.PasswordHash))
            {
                return Unauthorized("Invalid username or password.");
            }

            // JWT Token generation
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id ?? string.Empty)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenString });
        }
    }
}

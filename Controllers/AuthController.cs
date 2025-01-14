using library_management_api.Data;
using library_management_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace library_management_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LibraryDBContext dbContext;
        private readonly IConfiguration configuration;

        public AuthController(LibraryDBContext context, IConfiguration config)
        {
            dbContext = context;
            configuration = config;
        }

        [HttpPost("signup")]
        public IActionResult SignUp(User user)
        {
            dbContext.Users.Add(user);
            try
            {
                dbContext.SaveChanges();
            }
            catch
            {
                return Conflict("Username already exists");
            }
            return Ok("User registered successfully");
        }

        [HttpPost("signin")]
        public IActionResult SignIn(User user)
        {
            var existingUser = dbContext.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            if (existingUser == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            string? s = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");
            var key = Encoding.UTF8.GetBytes(s);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Sid, existingUser.Id.ToString())
                ]),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

        [HttpGet("verify")]
        [Authorize]
        public IActionResult VerifyToken()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return Unauthorized("Invalid token");
            }

            var user = dbContext.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var userDetails = new
            {
                user.Id,
                user.Username
            };

            return Ok(userDetails);
        }
    }
}

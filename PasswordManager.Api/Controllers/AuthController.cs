using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PasswordManager.Core.Data;
using PasswordManager.Core.Models;
using PasswordManager.Core.Services;
using BCrypt.Net;

namespace PasswordManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;
        private readonly ApplicationDbContext _context;
        
        public AuthController(IConfiguration configuration, IPasswordService passwordService, ApplicationDbContext context)
        {
            _configuration = configuration;
            _passwordService = passwordService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterRequest request)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Email == request.Email);
            if (userExists)
            {
                return BadRequest("User already exists");
            }

            var user = new User
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong password");
            }

            return Ok("Login successful");
        }

        [HttpPost("validate-password")]
        public async Task<IActionResult> ValidatePassword([FromBody] string password)
        {
            var result = await _passwordService.ValidatePassword(password);
            return Ok(result);
        }
    }
} 
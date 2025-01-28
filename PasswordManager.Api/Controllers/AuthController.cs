using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PasswordManager.Core.Models;
using PasswordManager.Core.Services;

namespace PasswordManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;
        
        public AuthController(IConfiguration configuration, IPasswordService passwordService)
        {
            _configuration = configuration;
            _passwordService = passwordService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterRequest request)
        {
            // TODO: Validation, hachage du mot de passe, sauvegarde en base de données
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginRequest request)
        {
            // TODO: Vérification des credentials, génération du JWT token
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Core.Models;
using PasswordManager.Core.Services;

namespace PasswordManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordGeneratorController : ControllerBase
    {
        private readonly IPasswordGeneratorService _passwordGeneratorService;

        public PasswordGeneratorController(IPasswordGeneratorService passwordGeneratorService)
        {
            _passwordGeneratorService = passwordGeneratorService;
        }

        [HttpPost("generate")]
        public ActionResult<string> GeneratePassword(PasswordGeneratorOptions options)
        {
            try
            {
                var password = _passwordGeneratorService.GeneratePassword(options);
                return Ok(new { password });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("validate")]
        public ActionResult<bool> ValidatePassword([FromBody] string password)
        {
            var isValid = _passwordGeneratorService.ValidatePassword(password);
            return Ok(new { 
                isValid,
                message = isValid ? "Password meets security requirements" : "Password does not meet security requirements"
            });
        }
    }
} 
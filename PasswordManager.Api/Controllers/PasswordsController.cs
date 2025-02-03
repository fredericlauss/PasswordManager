using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Core.Data;
using PasswordManager.Core.Models;
using PasswordManager.Core.Services;
using System.Security.Claims;

namespace PasswordManager.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEncryptionService _encryptionService;
        private readonly IConfiguration _configuration;

        public PasswordsController(
            ApplicationDbContext context,
            IEncryptionService encryptionService,
            IConfiguration configuration)
        {
            _context = context;
            _encryptionService = encryptionService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoredPassword>>> GetPasswords([FromQuery] string? category = null)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var query = _context.StoredPasswords.Where(p => p.UserId.ToString() == userId);
            
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            var passwords = await query.ToListAsync();
            return Ok(passwords);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<StoredPassword>>> SearchPasswords([FromQuery] string query)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var passwords = await _context.StoredPasswords
                .Where(p => p.UserId.ToString() == userId &&
                    (p.Title.Contains(query) || p.Username.Contains(query) || p.Website.Contains(query)))
                .ToListAsync();

            return Ok(passwords);
        }

        [HttpPost]
        public async Task<ActionResult<StoredPassword>> CreatePassword(CreatePasswordRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var encryptedPassword = _encryptionService.Encrypt(
                request.Password,
                _configuration["AppSettings:EncryptionKey"]!
            );

            var storedPassword = new StoredPassword
            {
                Title = request.Title,
                Username = request.Username,
                EncryptedPassword = encryptedPassword,
                Website = request.Website,
                Notes = request.Notes,
                Category = request.Category,
                UserId = Guid.Parse(userId)
            };

            _context.StoredPasswords.Add(storedPassword);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPassword), new { id = storedPassword.Id }, storedPassword);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StoredPassword>> GetPassword(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var password = await _context.StoredPasswords
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId.ToString() == userId);

            if (password == null) return NotFound();

            return Ok(password);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePassword(Guid id, UpdatePasswordRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var password = await _context.StoredPasswords
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId.ToString() == userId);

            if (password == null) return NotFound();

            password.Title = request.Title;
            password.Username = request.Username;
            password.Website = request.Website;
            password.Notes = request.Notes;
            password.Category = request.Category;
            password.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(request.Password))
            {
                password.EncryptedPassword = _encryptionService.Encrypt(
                    request.Password,
                    _configuration["AppSettings:EncryptionKey"]!
                );
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassword(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var password = await _context.StoredPasswords
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId.ToString() == userId);

            if (password == null) return NotFound();

            _context.StoredPasswords.Remove(password);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 
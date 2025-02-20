using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Core.Data;
using PasswordManager.Core.Models;
using PasswordManager.Core.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

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

            Console.WriteLine($"Searching passwords for user: {userId}");

            var query = _context.StoredPasswords
                .AsNoTracking()
                .Where(p => p.UserId == Guid.Parse(userId));
            
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            var passwords = await query.ToListAsync();
            
            // Décrypter les mots de passe avant de les envoyer
            foreach (var password in passwords)
            {
                password.EncryptedPassword = _encryptionService.Decrypt(
                    password.EncryptedPassword,
                    _configuration["AppSettings:EncryptionKey"]!
                );
            }
            
            Console.WriteLine($"Found {passwords.Count} passwords");
            
            return Ok(passwords);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<StoredPassword>>> SearchPasswords([FromQuery] string query)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) return Unauthorized();

                var userGuid = Guid.Parse(userId);
                Console.WriteLine($"Searching passwords for user {userId} with query: {query}");

                // Récupérer les mots de passe de l'utilisateur et filtrer en mémoire
                var passwords = await _context.StoredPasswords
                    .Where(p => p.UserId == userGuid)
                    .ToListAsync();

                var filteredPasswords = passwords.Where(p =>
                    p.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    p.Username.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    p.Website.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    p.Category.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                // Décrypter les mots de passe avant de les envoyer
                foreach (var password in filteredPasswords)
                {
                    password.EncryptedPassword = _encryptionService.Decrypt(
                        password.EncryptedPassword,
                        _configuration["AppSettings:EncryptionKey"]!
                    );
                }

                Console.WriteLine($"Found {filteredPasswords.Count} passwords matching the query");

                return Ok(filteredPasswords);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SearchPasswords: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<StoredPassword>> CreatePassword(CreatePasswordRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var user = await _context.Users.FindAsync(Guid.Parse(userId));
            if (user == null)
            {
                return BadRequest("User not found");
            }

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
                UserId = user.Id
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

            Console.WriteLine($"Looking for password {id} for user {userId}");

            var password = await _context.StoredPasswords
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == Guid.Parse(userId));

            if (password == null)
            {
                var exists = await _context.StoredPasswords.AnyAsync(p => p.Id == id);
                if (exists)
                {
                    return Forbid();
                }
                return NotFound();
            }

            return Ok(password);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePassword(Guid id, [FromBody] UpdatePasswordRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) return Unauthorized();

                Console.WriteLine($"Updating password {id} for user {userId}"); // Debug log

                var password = await _context.StoredPasswords
                    .FirstOrDefaultAsync(p => p.Id == id && p.UserId == Guid.Parse(userId));

                if (password == null)
                {
                    Console.WriteLine($"Password {id} not found for user {userId}"); // Debug log
                    return NotFound();
                }

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
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating password: {ex}"); // Debug log
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassword(Guid id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) return Unauthorized();

                Console.WriteLine($"Deleting password {id} for user {userId}"); // Debug log

                var password = await _context.StoredPasswords
                    .FirstOrDefaultAsync(p => p.Id == id && p.UserId == Guid.Parse(userId));

                if (password == null)
                {
                    Console.WriteLine($"Password {id} not found for user {userId}"); // Debug log
                    return NotFound();
                }

                _context.StoredPasswords.Remove(password);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting password: {ex}"); // Debug log
                return StatusCode(500, "Internal server error");
            }
        }
    }
} 
namespace PasswordManager.Core.Models
{
    public class StoredPassword
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string EncryptedPassword { get; set; } = string.Empty;
        public string? Website { get; set; }
        public string? Notes { get; set; }
        public string Category { get; set; } = "General";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }

    public class CreatePasswordRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Website { get; set; }
        public string? Notes { get; set; }
        public string Category { get; set; } = "General";
    }

    public class UpdatePasswordRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string? Website { get; set; }
        public string? Notes { get; set; }
        public string Category { get; set; } = "General";
    }
} 
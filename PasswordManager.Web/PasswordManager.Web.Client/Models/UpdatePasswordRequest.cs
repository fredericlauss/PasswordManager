using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Web.Client.Models
{
    public class UpdatePasswordRequest
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        public string? Password { get; set; }

        public string Website { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
    }
}
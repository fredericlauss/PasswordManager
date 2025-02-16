using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Web.Client.Models
{
    public class CreatePasswordRequest
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        public string Website { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
    }
}
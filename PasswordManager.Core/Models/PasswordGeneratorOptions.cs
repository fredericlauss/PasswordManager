using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Core.Models
{
    public class PasswordGeneratorOptions
    {
        [Range(8, 32, ErrorMessage = "Password length must be between 8 and 32 characters")]
        public int Length { get; set; } = 16;
        
        public bool IncludeUppercase { get; set; } = true;
        public bool IncludeLowercase { get; set; } = true;
        public bool IncludeNumbers { get; set; } = true;
        public bool IncludeSpecialChars { get; set; } = true;
        public string ExcludeCharacters { get; set; } = "";
    }
} 
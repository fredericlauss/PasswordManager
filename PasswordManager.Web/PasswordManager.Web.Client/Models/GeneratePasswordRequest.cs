namespace PasswordManager.Web.Client.Models
{
    public class GeneratePasswordRequest
    {
        public int Length { get; set; } = 16; // Valeur par défaut
        public bool IncludeUppercase { get; set; } = true;
        public bool IncludeLowercase { get; set; } = true;
        public bool IncludeNumbers { get; set; } = true;
        public bool IncludeSpecialChars { get; set; } = true;
        public string ExcludeCharacters { get; set; } = "";
    }
} 
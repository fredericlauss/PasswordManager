namespace PasswordManager.Core.Models
{
    public class PasswordGeneratorOptions
    {
        public int Length { get; set; } = 12;
        public bool IncludeUppercase { get; set; } = true;
        public bool IncludeLowercase { get; set; } = true;
        public bool IncludeNumbers { get; set; } = true;
        public bool IncludeSpecialChars { get; set; } = true;
        public string ExcludeCharacters { get; set; } = "";
    }
} 
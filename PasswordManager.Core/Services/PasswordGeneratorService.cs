using PasswordManager.Core.Models;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Core.Services
{
    public interface IPasswordGeneratorService
    {
        string GeneratePassword(PasswordGeneratorOptions options);
        bool ValidatePassword(string password);
    }

    public class PasswordGeneratorService : IPasswordGeneratorService
    {
        private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        private const string NumberChars = "0123456789";
        private const string SpecialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        public string GeneratePassword(PasswordGeneratorOptions options)
        {
            var validChars = new StringBuilder();
            var password = new StringBuilder();

            // Construire l'ensemble des caractères valides
            if (options.IncludeUppercase) validChars.Append(UppercaseChars);
            if (options.IncludeLowercase) validChars.Append(LowercaseChars);
            if (options.IncludeNumbers) validChars.Append(NumberChars);
            if (options.IncludeSpecialChars) validChars.Append(SpecialChars);

            // Retirer les caractères exclus
            foreach (char c in options.ExcludeCharacters)
            {
                validChars.Replace(c.ToString(), "");
            }

            if (validChars.Length == 0)
                throw new ArgumentException("No valid characters available for password generation");

            // S'assurer que le mot de passe contient au moins un caractère de chaque type requis
            if (options.IncludeUppercase)
                password.Append(GetRandomChar(UppercaseChars));
            if (options.IncludeLowercase)
                password.Append(GetRandomChar(LowercaseChars));
            if (options.IncludeNumbers)
                password.Append(GetRandomChar(NumberChars));
            if (options.IncludeSpecialChars)
                password.Append(GetRandomChar(SpecialChars));

            // Remplir le reste du mot de passe
            while (password.Length < options.Length)
            {
                password.Append(GetRandomChar(validChars.ToString()));
            }

            // Mélanger le mot de passe
            return new string(password.ToString().ToCharArray().OrderBy(x => RandomNumberGenerator.GetInt32(password.Length)).ToArray());
        }

        private static char GetRandomChar(string chars)
        {
            return chars[RandomNumberGenerator.GetInt32(chars.Length)];
        }

        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasNumber = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(c => SpecialChars.Contains(c));

            return hasUpper && hasLower && hasNumber && hasSpecial;
        }
    }
} 
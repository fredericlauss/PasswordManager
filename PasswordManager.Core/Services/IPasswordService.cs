namespace PasswordManager.Core.Services
{
    public interface IPasswordService
    {
        Task<bool> ValidatePassword(string password);
        // ... autres méthodes
    }

    public class PasswordService : IPasswordService
    {
        public async Task<bool> ValidatePassword(string password)
        {
            // Implémentation
            return true;
        }
    }
} 
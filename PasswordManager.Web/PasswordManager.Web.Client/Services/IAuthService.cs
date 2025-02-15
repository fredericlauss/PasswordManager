using PasswordManager.Web.Client.Models;

namespace PasswordManager.Web.Client.Services
{
    public interface IAuthService
    {
        Task<bool> Login(LoginRequest request);
        Task<bool> Register(RegisterRequest request);
        Task Logout();
        Task<string?> GetToken();
    }
}
using System.Net.Http.Json;
using PasswordManager.Web.Client.Models;

namespace PasswordManager.Web.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> Login(LoginRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", request);
            return response.IsSuccessStatusCode;
        }

public async Task<bool> Register(RegisterRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("http://localhost:5001/api/Auth/register", request);
            Console.WriteLine($"Register response: {response.StatusCode}");
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Register error: {error}");
            }
            
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Register exception: {ex}");
            throw;
        }
    }

        public async Task Logout()
        {
            // Nettoyer les données d'authentification si nécessaire
        }
    }
}
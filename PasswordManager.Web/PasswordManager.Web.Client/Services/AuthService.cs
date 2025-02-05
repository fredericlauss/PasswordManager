using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using PasswordManager.Web.Client.Models;
using Blazored.LocalStorage;

namespace PasswordManager.Web.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(
            HttpClient http, 
            AuthenticationStateProvider authStateProvider,
            ILocalStorageService localStorage)
        {
            _http = http;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<bool> Login(LoginRequest request)
        {
            var response = await _http.PostAsJsonAsync("http://localhost:5001/api/Auth/login", request);
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                {
                    // Stockage du token
                    await _localStorage.SetItemAsync("authToken", loginResponse.Token);
                    
                    // Mise à jour de l'état d'authentification
                    ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(loginResponse.Token);
                    return true;
                }
            }
            return false;
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

    public class LoginResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
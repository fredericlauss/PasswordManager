using System.Net.Http.Json;
using Blazored.LocalStorage;
using PasswordManager.Core.Models;

namespace PasswordManager.Web.Client.Services
{
    public class PasswordService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public PasswordService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        public async Task<List<StoredPassword>> GetPasswords(string? category = null)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var url = category == null ? "api/passwords" : $"api/passwords?category={category}";
                var response = await _http.GetFromJsonAsync<List<StoredPassword>>(url);
                return response ?? new List<StoredPassword>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching passwords: {ex.Message}");
                return new List<StoredPassword>();
            }
        }
    }
} 
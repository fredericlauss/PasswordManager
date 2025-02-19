using System.Net.Http.Json;
using Blazored.LocalStorage;
using PasswordManager.Core.Models;
using PasswordManager.Web.Client.Models;

namespace PasswordManager.Web.Client.Services
{
    public class PasswordService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private const string BaseUrl = "http://localhost:5001/api/Passwords";

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

                var url = category == null ? BaseUrl : $"{BaseUrl}?category={category}";
                var response = await _http.GetFromJsonAsync<List<StoredPassword>>(url);
                return response ?? new List<StoredPassword>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching passwords: {ex.Message}");
                return new List<StoredPassword>();
            }
        }

        public async Task<StoredPassword?> GetPassword(Guid id)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                return await _http.GetFromJsonAsync<StoredPassword>($"{BaseUrl}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching password: {ex.Message}");
                return null;
            }
        }

        public async Task<List<StoredPassword>> SearchPasswords(string searchTerm)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _http.GetFromJsonAsync<List<StoredPassword>>($"{BaseUrl}/search?query={Uri.EscapeDataString(searchTerm)}");
                return response ?? new List<StoredPassword>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching passwords: {ex.Message}");
                return new List<StoredPassword>();
            }
        }

        public async Task<bool> CreatePassword(PasswordManager.Core.Models.CreatePasswordRequest request)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _http.PostAsJsonAsync(BaseUrl, request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating password: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdatePassword(Guid id, PasswordManager.Core.Models.UpdatePasswordRequest request)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _http.PutAsJsonAsync($"{BaseUrl}/{id}", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating password: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeletePassword(Guid id)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _http.DeleteAsync($"{BaseUrl}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting password: {ex.Message}");
                return false;
            }
        }

        public async Task<string> GeneratePassword(GeneratePasswordRequest request)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _http.PostAsJsonAsync("http://localhost:5001/api/PasswordGenerator/generate", request);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<GeneratedPasswordResponse>();
                    return result?.Password ?? string.Empty;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating password: {ex.Message}");
                return string.Empty;
            }
        }
    }
} 
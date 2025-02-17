using System.Net.Http.Json;
using Blazored.LocalStorage;
using PasswordManager.Core.Models;

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

                var response = await _http.GetFromJsonAsync<List<StoredPassword>>($"{BaseUrl}/search?query={searchTerm}");
                return response ?? new List<StoredPassword>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching passwords: {ex.Message}");
                return new List<StoredPassword>();
            }
        }

        public async Task<bool> CreatePassword(CreatePasswordRequest request)
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

        public async Task<bool> UpdatePassword(Guid id, UpdatePasswordRequest request)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _http.PutAsJsonAsync($"{BaseUrl}/{id}", request);
                Console.WriteLine($"Update response: {response.StatusCode}"); // Debug
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

                Console.WriteLine($"Deleting password with ID: {id}"); // Debug
                var response = await _http.DeleteAsync($"{BaseUrl}/{id}");
                Console.WriteLine($"Delete response: {response.StatusCode}"); // Debug
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting password: {ex.Message}");
                return false;
            }
        }
    }
} 
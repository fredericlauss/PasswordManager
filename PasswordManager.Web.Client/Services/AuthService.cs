public interface IAuthService
{
    Task<bool> Login(LoginRequest request);
    Task<bool> Register(RegisterRequest request);
    Task Logout();
}

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
        var response = await _http.PostAsJsonAsync("api/auth/register", request);
        return response.IsSuccessStatusCode;
    }

    public async Task Logout()
    {
        // Nettoyer le token JWT et autres données d'authentification
    }
} 
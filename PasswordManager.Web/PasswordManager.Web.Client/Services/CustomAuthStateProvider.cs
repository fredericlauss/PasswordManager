using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationState _anonymous;

    public CustomAuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrEmpty(token))
                return _anonymous;

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwt.Claims, "jwt");
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }
        catch (Exception)
        {
            // TODO: Log l'erreur
            // En cas d'erreur (par exemple, pendant le prérendu), retourner l'état anonyme
            return _anonymous;
        }
    }

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void NotifyUserAuthentication(string token)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(_anonymous);
        NotifyAuthenticationStateChanged(authState);
    }
} 
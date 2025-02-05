using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Blazored.LocalStorage;

namespace PasswordManager.Web.Client.Models
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private Task<AuthenticationState>? _authenticationStateTask;
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            // Initialiser avec un état non authentifié par défaut
            _authenticationStateTask = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                if (_authenticationStateTask == null)
                {
                    // Ne devrait jamais arriver, mais par sécurité
                    return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
                }

                return _authenticationStateTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAuthenticationStateAsync: {ex.Message}");
                // Retourner un état non authentifié en cas d'erreur
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }
        }

        public async void NotifyUserAuthentication(string token)
        {
            try
            {
                var claims = ParseClaimsFromJwt(token);
                var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
                var authState = new AuthenticationState(authenticatedUser);
                _authenticationStateTask = Task.FromResult(authState);
                NotifyAuthenticationStateChanged(_authenticationStateTask);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in NotifyUserAuthentication: {ex.Message}");
            }
        }

        public void NotifyUserLogout()
        {
            try
            {
                var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
                var authState = new AuthenticationState(anonymousUser);
                _authenticationStateTask = Task.FromResult(authState);
                NotifyAuthenticationStateChanged(_authenticationStateTask);
                _localStorage.RemoveItemAsync("authToken");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in NotifyUserLogout: {ex.Message}");
            }
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt);
                return token.Claims;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing JWT claims: {ex.Message}");
                return new List<Claim>();
            }
        }
    }
}
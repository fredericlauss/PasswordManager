using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using PasswordManager.Web.Client.Services;
using PasswordManager.Web.Client.Models;
using Blazored.LocalStorage;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        // Configuration des services
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        builder.Services.AddAuthorizationCore();
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<PasswordService>();
        
        builder.Services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri(builder.Configuration["ApiUrl"] ?? builder.HostEnvironment.BaseAddress)
        });

        var host = builder.Build();

        // Vérification que les services sont correctement configurés
        var authService = host.Services.GetRequiredService<IAuthService>();
        var httpClient = host.Services.GetRequiredService<HttpClient>();

        Console.WriteLine($"API URL configured as: {httpClient.BaseAddress}");

        await host.RunAsync();
    }
}
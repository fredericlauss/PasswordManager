var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri(builder.Configuration["ApiUrl"] ?? builder.HostEnvironment.BaseAddress) 
});

// Ajout des services nécessaires pour l'authentification
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IPasswordService, PasswordService>();

await builder.Build().RunAsync(); 
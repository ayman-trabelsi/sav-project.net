using BlazorFrontend;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register AuthorizedHttpClientHandler
builder.Services.AddTransient<AuthorizedHttpClientHandler>();

// Configure named HttpClient with BaseAddress + Authorization handler
builder.Services.AddHttpClient("AuthorizedClient", client =>
    {
        client.BaseAddress = new Uri("http://localhost:5087"); // your backend URL
    })
    .AddHttpMessageHandler<AuthorizedHttpClientHandler>();

// Register AuthService with injected named HttpClient + IJSRuntime
builder.Services.AddScoped<AuthService>(sp =>
{
    var client = sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthorizedClient");
    var js = sp.GetRequiredService<IJSRuntime>();
    return new AuthService(client, js);
});

// Ensure globally injected HttpClient (via @inject) uses the authorized one
builder.Services.AddScoped<HttpClient>(sp =>
{
    return sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthorizedClient");
});

// Register AuthenticationStateProvider
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<JwtAuthenticationStateProvider>());

// Build the host
var host = builder.Build();

// Load token and set auth header before running the app
var authService = host.Services.GetRequiredService<AuthService>();
await authService.LoadUserFromTokenAsync();

// Run the app
await host.RunAsync();
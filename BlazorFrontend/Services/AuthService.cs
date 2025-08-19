using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Shared.ModelsDto;
using System;
using System.Linq;
using System.Security.Claims;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private const string TokenKey = "authToken";

    public string[] Roles { get; private set; }
    public string Username { get; private set; }
    public event Action OnAuthStateChanged;

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var loginRequest = new { Email = email, Password = password };
        var content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/auth/login", content);
        if (!response.IsSuccessStatusCode)
            return false;

        var responseContent = await response.Content.ReadAsStringAsync();
        var loginResponse = JsonSerializer.Deserialize<AuthResponseDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, loginResponse.Token);

            DecodeAndStoreRoles(loginResponse.Token);
            OnAuthStateChanged?.Invoke();
            return true;
        }

        return false;
    }

    public async Task<bool> RegisterAsync(string email, string name, string password)
    {
        var registerRequest = new { Email = email, Name = name, Password = password };
        var content = new StringContent(JsonSerializer.Serialize(registerRequest), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/auth/register", content);
        return response.IsSuccessStatusCode;
    }

    public async Task LogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        _httpClient.DefaultRequestHeaders.Authorization = null;
        Roles = null;
        OnAuthStateChanged?.Invoke();
    }

    public async Task LoadUserFromTokenAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            DecodeAndStoreRoles(token);
        }
        else
        {
            Roles = null;
        }

        OnAuthStateChanged?.Invoke();
    }

    public async Task<string> GetTokenAsync()
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
    }

    private void DecodeAndStoreRoles(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        Roles = jwt.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
        Username = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
    }
}

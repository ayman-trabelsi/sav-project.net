using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly AuthService _authService;
    public JwtAuthenticationStateProvider(AuthService authService)
    {
        _authService = authService;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _authService.GetTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        // Parse the token and create a ClaimsPrincipal
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }
    public void NotifyUserAuthentication(string token)
    {
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }
    public void NotifyUserLogout()
    {
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }
    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = Convert.FromBase64String(Base64UrlDecode(payload));
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonBytes);

        foreach (var kvp in keyValuePairs)
        {
            if (kvp.Key == "role" || kvp.Key == "roles" || kvp.Key == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
            {
                if (kvp.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var role in kvp.Value.EnumerateArray())
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.GetString()));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, kvp.Value.GetString()));
                }
            }
            else if (kvp.Key == "name")
            {
                claims.Add(new Claim(ClaimTypes.Name, kvp.Value.GetString()));
            }
            else
            {
                claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
            }
        }
        return claims;
    }


    private string Base64UrlDecode(string base64Url)
    {
        int mod4 = base64Url.Length % 4;
        if (mod4 == 2) base64Url += "==";
        else if (mod4 == 3) base64Url += "=";

        base64Url = base64Url.Replace('-', '+').Replace('_', '/');
        return base64Url;
    }




}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public static class JwtHelper
{
    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        return token.Claims;
    }

    public static string[] GetRolesFromJwt(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        return jwt.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
    }
}
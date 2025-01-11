using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using MiniProjet.Repository.IRepository;
using OCRAppApi.Context;
using MiniProjet.ModelsDto;
using MiniProjet.Models;

namespace MiniProjet.Repository
{
    public class AuthentificationRepository : IAuthentificationRepository

    {
        public readonly ApplicationDbContext _context;
        public readonly IConfiguration _configuration;
        public AuthentificationRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string Authentifier(UtilisateurDto _userData)
        {
            if (_userData != null)
            {
                IQueryable<Client> queryClient = _context.Clients;
                IQueryable<ResponsableSAV> queryResponsable = _context.ResponsableSAV;

                bool isResponsable = queryResponsable.Any(r => r.USerName == _userData.NomUtilisateur);

                User utilisateur = isResponsable
                 ? queryResponsable
                     .FirstOrDefault(e => e.USerName == _userData.NomUtilisateur && e.Password == _userData.MotPasse)
                 : queryClient
                     .FirstOrDefault(e => e.USerName == _userData.NomUtilisateur && e.Password == _userData.MotPasse);
                string role = isResponsable ? "Responsable" : "Client";

                if (utilisateur == null)
                {
                    return null;
                }
                else
                {
                var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.Name, _userData.NomUtilisateur),
                new Claim(ClaimTypes.Role, role) 

               
                };

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Issuer = _configuration["Jwt:Issuer"],
                        Audience = _configuration["Jwt:Audience"],
                        Expires = DateTime.UtcNow.AddMinutes(500),
                        SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                        Subject = new ClaimsIdentity(claims)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var _token = tokenHandler.WriteToken(token);

                    return _token;
                }
            }
            else
            {
                return null;
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Aplicacion.Interfaces.Infraestructura;

namespace Infraestructura.Seguridad
{
    public class TokenService : ITokenService
    {
        private readonly TokenSettings _settings;

        public TokenService(IOptions<TokenSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateToken(string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, email)
        };

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

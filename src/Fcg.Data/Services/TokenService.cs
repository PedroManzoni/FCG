using Fcg.Domain.Entities;
using Fcg.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fcg.Data.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        // 1. Pega as configurações do appsettings.json
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"]!);

        // 2. Define as CLAIMS (informações que vão no token)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // ID do usuário
            new Claim(ClaimTypes.Name, user.Name),                     // Nome
            new Claim(ClaimTypes.Email, user.Email),                   // Email
            new Claim(ClaimTypes.Role, user.Role)                      // Role (Admin ou User)
        };

        // 3. Cria as credenciais de assinatura
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        // 4. Gera o token
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // 5. Retorna o token como string
        return tokenHandler.WriteToken(token);
    }
}

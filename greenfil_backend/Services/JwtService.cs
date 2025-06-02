using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using greenfil_backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace Greenfil.Backend.Services;

public class JwtService
{
    private readonly string claveSecreta;

    public JwtService(IConfiguration config)
    {
        claveSecreta = config["Jwt:Key"];
    }

    public string GenerateToken(usuario usuario) // Clase en plural
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveSecreta));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
 
        var token = new JwtSecurityToken(
            issuer: "tuApi",
            audience: "tuApi",
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

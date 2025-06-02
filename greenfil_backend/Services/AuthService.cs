using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using greenfil_backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace Greenfil.Backend.Services;

public class AuthService
{
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(object user)
    {
        string id = "";
        string rol = "";
        string email = "";

        if (user is usuariosback ub)
        {
            id = ub.Id.ToString();
            rol = ub.Rol;
            email = ub.Usuario;
        }
        else
        {
            throw new ArgumentException("Tipo de usuario no compatible.");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, rol)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
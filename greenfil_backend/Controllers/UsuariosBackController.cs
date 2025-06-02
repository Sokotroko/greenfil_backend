using greenfil_backend.DTOs;
using greenfil_backend.Models;
using Greenfil.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace greenfil_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuariosBackController : ControllerBase
{
    private readonly GreenfilContext _context;
    private readonly AuthService _authService;

    public UsuariosBackController(GreenfilContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginBackDto loginDto)
    {
        var usuario = await _context.usuariosbacks
            .FirstOrDefaultAsync(u => u.Usuario == loginDto.Usuario && u.PasswordHash == loginDto.PasswordHash);

        if (usuario == null)
            return Unauthorized("Credenciales inválidas.");

        var token = _authService.GenerateToken(usuario);
        return Ok(new { token });
    }

    [HttpPost("crear")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CrearUsuario([FromBody] usuariosback nuevo)
    {
        var existe = await _context.usuariosbacks.AnyAsync(u => u.Usuario == nuevo.Usuario);
        if (existe)
            return BadRequest("El usuario ya existe.");

        _context.usuariosbacks.Add(nuevo);
        await _context.SaveChangesAsync();
        return Ok(nuevo);
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<IEnumerable<usuariosback>>> GetUsuarios()
    {
        return await _context.usuariosbacks.ToListAsync();
    }
}
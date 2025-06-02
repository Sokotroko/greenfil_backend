using greenfil_backend.DTOs;
using greenfil_backend.Models;
<<<<<<< HEAD
using greenfil_backend.DTOs;
using Greenfil.Backend.Services;
=======
using Greenfil.Backend.Services;
using Microsoft.AspNetCore.Authorization;
>>>>>>> f5250b035c6cb6a7ddff097203ecd55f48c62c60
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace greenfil_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly GreenfilContext _context;
<<<<<<< HEAD
        private readonly JwtService _jwtService;

        public UsuariosController(GreenfilContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
=======
        private readonly PasswordService _passwordService;

        public UsuariosController(GreenfilContext context, PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
>>>>>>> f5250b035c6cb6a7ddff097203ecd55f48c62c60
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<usuario>>> GetUsuarios()
        {
            return await _context.usuarios
                .Include(u => u.modelo3ds)
                .Include(u => u.pedidos)
                .ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<usuario>> GetUsuario(int id)
        {
            var usuario = await _context.usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // POST: api/Usuarios  -  se agrego cifrado de contraseñas
        [HttpPost]
<<<<<<< HEAD
        public async Task<ActionResult<UsuarioRespuestaDTO>> PostUsuario(usuario usuario)
=======
        public async Task<ActionResult<usuario>> Postusuario(usuario nuevo)
>>>>>>> f5250b035c6cb6a7ddff097203ecd55f48c62c60
        {
            // Verifica que el email o usuario no existan
            var existe = await _context.usuarios.AnyAsync(u => u.Email == nuevo.Email);
            if (existe)
                return BadRequest("El correo ya está en uso.");

            // Hashear la contraseña antes de guardar
            nuevo.PasswordHash = _passwordService.HashPassword(nuevo.PasswordHash);

            _context.usuarios.Add(nuevo);
            await _context.SaveChangesAsync();

<<<<<<< HEAD
            var token = _jwtService.GenerateToken(usuario);

            var respuesta = new UsuarioRespuestaDTO
            {
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                Rol = usuario.Rol,
                Token = token
            };

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, respuesta);
=======
            return CreatedAtAction("Getusuario", new { id = nuevo.Id }, nuevo);
>>>>>>> f5250b035c6cb6a7ddff097203ecd55f48c62c60
        }
        
        //login para contraseñas hasheadas
        [HttpPost("login")]
        public async Task<IActionResult> LoginCliente([FromBody] LoginDto dto)
        {
            var user = await _context.usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return Unauthorized("Correo no encontrado.");

            var passwordOk = _passwordService.VerifyPassword(user.PasswordHash, dto.Password);

            if (!passwordOk)
                return Unauthorized("Contraseña incorrecta.");

            return Ok(new
            {
                id = user.Id,
                nombreUsuario = user.NombreUsuario,
                email = user.Email,
                puntos = user.Puntos
            });
        }
        
        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutUsuario(int id, usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.usuarios.Any(e => e.Id == id);
        }
    }
}
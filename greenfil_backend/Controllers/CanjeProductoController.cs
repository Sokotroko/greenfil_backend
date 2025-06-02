using System.Security.Claims;
using greenfil_backend.DTOs;
using greenfil_backend.Models;
using Greenfil.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace greenfil_backend.Controllers;


    [ApiController]
    [Route("api/[controller]")]
    public class CanjeProductoController : ControllerBase
    {
        private readonly CanjeProductoService _service;
        private readonly GreenfilContext _context;

        public CanjeProductoController(CanjeProductoService service, GreenfilContext context)
        {
            _service = service;
            _context = context;
        }

        [Authorize]
        [HttpPost("canjear")]
        public async Task<IActionResult> Canjear([FromBody] CanjeProductoDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuario no autenticado.");

            int usuarioId = int.Parse(userIdClaim.Value);
            var resultado = await _service.CanjearAsync(usuarioId, dto.ProductoId, dto.Cantidad);

            if (resultado == "Canje exitoso.")
                return Ok(new { mensaje = resultado });

            return BadRequest(new { error = resultado });
        }

        [Authorize(Roles = "admin")]
        [HttpGet("pendientes")]
        public async Task<IActionResult> GetPendientes()
        {
            var pendientes = await _context.pedidos
                .Include(p => p.detallepedidos)
                .ThenInclude(dp => dp.Producto)
                .Where(p => p.Estado == "pendiente")
                .ToListAsync();

            return Ok(pendientes);
        }
    }


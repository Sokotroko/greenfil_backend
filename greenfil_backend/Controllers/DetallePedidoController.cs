
using greenfil_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace greenfil_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallePedidoController : ControllerBase
    {
        private readonly GreenfilContext _context;

        public DetallePedidoController(GreenfilContext context)
        {
            _context = context;
        }

        // GET: api/DetallePedido
        [HttpGet]
        public async Task<ActionResult<IEnumerable<detallepedido>>> GetDetallePedidos()
        {
            return await _context.detallepedidos
                .Include(d => d.Producto)
                .Include(d => d.Pedido)
                .ToListAsync();
        }

        // GET: api/DetallePedido/5
        [HttpGet("{id}")]
        public async Task<ActionResult<detallepedido>> GetDetallePedido(int id)
        {
            var detalle = await _context.detallepedidos
                .Include(d => d.Producto)
                .Include(d => d.Pedido)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (detalle == null)
            {
                return NotFound();
            }

            return detalle;
        }

        // POST: api/DetallePedido
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<detallepedido>> PostDetallePedido(detallepedido detalle)
        {
            _context.detallepedidos.Add(detalle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDetallePedido), new { id = detalle.Id }, detalle);
        }

        // PUT: api/DetallePedido/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutDetallePedido(int id, detallepedido detalle)
        {
            if (id != detalle.Id)
            {
                return BadRequest();
            }

            _context.Entry(detalle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetallePedidoExists(id))
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

        // DELETE: api/DetallePedido/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteDetallePedido(int id)
        {
            var detalle = await _context.detallepedidos.FindAsync(id);
            if (detalle == null)
            {
                return NotFound();
            }

            _context.detallepedidos.Remove(detalle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetallePedidoExists(int id)
        {
            return _context.detallepedidos.Any(e => e.Id == id);
        }
    }
}

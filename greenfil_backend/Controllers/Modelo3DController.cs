
using greenfil_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace greenfil_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Modelo3DController : ControllerBase
    {
        private readonly GreenfilContext _context;

        public Modelo3DController(GreenfilContext context)
        {
            _context = context;
        }

        // GET: api/Modelo3D
        [HttpGet]
        public async Task<ActionResult<IEnumerable<modelo3d>>> GetModelos()
        {
            return await _context.modelo3ds
                .Include(m => m.Usuario)
                .ToListAsync();
        }

        // GET: api/Modelo3D/5
        [HttpGet("{id}")]
        public async Task<ActionResult<modelo3d>> GetModelo(int id)
        {
            var modelo = await _context.modelo3ds
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (modelo == null)
            {
                return NotFound();
            }

            return modelo;
        }

        // POST: api/Modelo3D
        [HttpPost]
        public async Task<ActionResult<modelo3d>> PostModelo(modelo3d modelo)
        {
            _context.modelo3ds.Add(modelo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetModelo), new { id = modelo.Id }, modelo);
        }

        // PUT: api/Modelo3D/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModelo(int id, modelo3d modelo)
        {
            if (id != modelo.Id)
            {
                return BadRequest();
            }

            _context.Entry(modelo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModeloExists(id))
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

        // DELETE: api/Modelo3D/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModelo(int id)
        {
            var modelo = await _context.modelo3ds.FindAsync(id);
            if (modelo == null)
            {
                return NotFound();
            }

            _context.modelo3ds.Remove(modelo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModeloExists(int id)
        {
            return _context.modelo3ds.Any(e => e.Id == id);
        }
    }
}

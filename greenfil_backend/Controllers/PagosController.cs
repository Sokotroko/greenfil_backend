using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using greenfil_backend.DTOs;

namespace greenfil_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagosController : ControllerBase
    {
        [Authorize] // Solo usuarios autenticados
        [HttpPost("subir-comprobante")]
        public async Task<IActionResult> SubirComprobante([FromForm] SubirComprobanteRequest request)
        {
            var file = request.Comprobante;
            var monto = request.Monto;

            if (file == null || file.Length == 0)
                return BadRequest("No se envió ningún comprobante.");

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Comprobantes");
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var path = Path.Combine(folder, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Puedes guardar en la base de datos aquí si lo deseas

            return Ok(new { mensaje = "✅ Comprobante recibido", monto });
        }
    }
}

using Greenfil.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Greenfil.Backend.Controllers;

[ApiController]
[Route("api/stl")]
[Produces("application/json")]
public class StlController : ControllerBase
{
    private readonly PythonService _pythonService;

    public StlController(PythonService pythonService)
    {
        _pythonService = pythonService;
    }

    [HttpPost("generate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateStl(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
            return BadRequest("No se envió ninguna imagen.");

        // Guarda la imagen temporalmente
        var extension = Path.GetExtension(imageFile.FileName);
        var fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
        var tempImagePath = Path.Combine(Path.GetTempPath(), $"{fileName}{extension}");

        await using (var stream = new FileStream(tempImagePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        try
        {
            // Llama al servicio Python
            var stlPath = await _pythonService.GenerateStlFromImage(tempImagePath);
            var stlBytes = await System.IO.File.ReadAllBytesAsync(stlPath);

            // Devuelve el STL como archivo descargable
            return File(stlBytes, "application/octet-stream", "modelo.stl");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al generar STL: {ex.Message}");
        }
        finally
        {
            // Limpia archivos temporales
            if (System.IO.File.Exists(tempImagePath))
                System.IO.File.Delete(tempImagePath);
        }
    }
}
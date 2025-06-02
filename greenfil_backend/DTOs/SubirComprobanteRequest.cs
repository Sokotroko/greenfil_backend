using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace greenfil_backend.DTOs
{
    public class SubirComprobanteRequest
    {
        [FromForm(Name = "comprobante")]
        public IFormFile Comprobante { get; set; }

        [FromForm(Name = "monto")]
        public string Monto { get; set; }
    }
}
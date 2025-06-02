using Greenfil.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace greenfil_backend.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _paymentService;

    public PaymentController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("charge")]
    public async Task<IActionResult> Charge([FromBody] PaymentRequest paymentRequest)
    {
        var charge = await _paymentService.CreateCharge(paymentRequest.Token, paymentRequest.Amount);
        if (charge.Status == "succeeded")
        {
            return Ok(charge);
        }
        return BadRequest("Error al procesar el pago.");
    }
}

public class PaymentRequest
{
    public string Token { get; set; }
    public decimal Amount { get; set; }
}

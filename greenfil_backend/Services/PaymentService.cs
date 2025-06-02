
using Stripe;

namespace Greenfil.Backend.Services;

public class PaymentService
{
    private readonly string _stripeSecretKey;

    public PaymentService(IConfiguration config)
    {
        _stripeSecretKey = config["Stripe:SecretKey"];
    }

    public async Task<Charge> CreateCharge(string token, decimal amount)
    {
        StripeConfiguration.ApiKey = _stripeSecretKey;

        var chargeOptions = new ChargeCreateOptions
        {
            Amount = (long)(amount * 100),
            Currency = "usd",
            Description = "Pago de Filamento",
            Source = token,
        };

        var chargeService = new ChargeService();
        Charge charge = await chargeService.CreateAsync(chargeOptions);
        return charge;
    }
}
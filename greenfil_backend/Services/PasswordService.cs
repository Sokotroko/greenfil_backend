using Microsoft.AspNetCore.Identity;

namespace Greenfil.Backend.Services;

public class PasswordService
{
    private readonly PasswordHasher<string> _hasher = new();

    public string HashPassword(string password)
    { 
        return _hasher.HashPassword("context", password);
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var result = _hasher.VerifyHashedPassword("context", hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}

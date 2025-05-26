namespace greenfil_backend.DTOs;

public class LoginBackDto
{
    public string Usuario { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
}
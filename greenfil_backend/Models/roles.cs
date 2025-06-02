namespace greenfil_backend.Models;

public class roles
{
    public int Id { get; set; }
    public string NombreRol { get; set; } = string.Empty;

    public ICollection<usuario> Usuarios { get; set; } = new List<usuario>();
}

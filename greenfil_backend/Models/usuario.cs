using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace greenfil_backend.Models;

[Index("Email", Name = "Email", IsUnique = true)]
public partial class usuario
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string NombreUsuario { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    public int? Puntos { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaRegistro { get; set; }
    
    public string Rol { get; set; } 

    [InverseProperty("Usuario")]
    public virtual ICollection<modelo3d> modelo3ds { get; set; } = new List<modelo3d>();

    [InverseProperty("Usuario")]
    public virtual ICollection<pedido> pedidos { get; set; } = new List<pedido>();
}

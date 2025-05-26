using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace greenfil_backend.Models;

[Table("usuariosback")]
[Index("Usuario", Name = "Usuario", IsUnique = true)]
public partial class usuariosback
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Usuario { get; set; } = null!;

    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(20)]
    public string Rol { get; set; } = null!;
}

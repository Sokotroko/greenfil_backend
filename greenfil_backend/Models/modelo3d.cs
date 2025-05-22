using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace greenfil_backend.Models;

[Table("modelo3d")]
[Index("UsuarioId", Name = "UsuarioId")]
public partial class modelo3d
{
    [Key]
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    [StringLength(100)]
    public string NombreModelo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime FechaCreacion { get; set; }

    [StringLength(255)]
    public string RutaArchivo { get; set; } = null!;

    [ForeignKey("UsuarioId")]
    [InverseProperty("modelo3ds")]
    public virtual usuario? Usuario { get; set; }

}

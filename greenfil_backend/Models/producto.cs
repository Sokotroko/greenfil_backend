using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace greenfil_backend.Models;

public partial class producto
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string NombreProducto { get; set; } = null!;

    [Column(TypeName = "enum('filamento','figura')")]
    public string Tipo { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? Descripcion { get; set; }

    [Precision(10, 2)]
    public decimal? PrecioSoles { get; set; }

    public int? PrecioPuntos { get; set; }

    public int? Stock { get; set; }

    [StringLength(255)]
    public string? Imagen { get; set; }

    [InverseProperty("Producto")]
    public virtual ICollection<detallepedido> detallepedidos { get; set; } = new List<detallepedido>();
}

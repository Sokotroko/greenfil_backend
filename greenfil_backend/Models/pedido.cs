using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace greenfil_backend.Models;

[Table("pedido")]
[Index("UsuarioId", Name = "UsuarioId")]
public partial class pedido
{
    [Key]
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Fecha { get; set; }

    [Precision(10, 2)]
    public decimal? TotalSoles { get; set; }

    public int? TotalPuntos { get; set; }

    [Column(TypeName = "enum('pendiente','entregado','cancelado')")]
    public string Estado { get; set; } = null!;

    [ForeignKey("UsuarioId")]
    [InverseProperty("pedidos")]
    
    public virtual usuario? Usuario { get; set; }

    [InverseProperty("Pedido")]
    public virtual ICollection<detallepedido> detallepedidos { get; set; } = new List<detallepedido>();
}

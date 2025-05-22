using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace greenfil_backend.Models;

[Table("detallepedido")]
[Index("PedidoId", Name = "PedidoId")]
[Index("ProductoId", Name = "ProductoId")]
public partial class detallepedido
{
    [Key]
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    [Precision(10, 2)]
    public decimal? SubtotalSoles { get; set; }

    public int? SubtotalPuntos { get; set; }

    [ForeignKey("PedidoId")]
    [InverseProperty("detallepedidos")]
    public virtual pedido? Pedido { get; set; }

    [ForeignKey("ProductoId")]
    [InverseProperty("detallepedidos")]
    public virtual producto? Producto { get; set; }
}

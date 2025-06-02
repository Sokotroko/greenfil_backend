using greenfil_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Greenfil.Backend.Services;

public class CanjeProductoService
{
    private readonly GreenfilContext _context;

    public CanjeProductoService(GreenfilContext context)
    {
        _context = context;
    }

    public async Task<string> CanjearAsync(int usuarioId, int productoId, int cantidad)
    {
        var usuario = await _context.usuarios.FindAsync(usuarioId);
        if (usuario == null)
            return "Usuario no encontrado.";

        var producto = await _context.productos.FirstOrDefaultAsync(p => p.Id == productoId && p.Tipo == "filamento");
        if (producto == null)
            return "Producto de tipo 'filamento' no encontrado.";

        if (producto.Stock < cantidad)
            return "Stock insuficiente.";

        int puntosRequeridos = (producto.PrecioPuntos ?? 0) * cantidad;
        if (usuario.Puntos < puntosRequeridos)
            return "Puntos insuficientes.";

        var pedido = new pedido
        {
            UsuarioId = usuarioId,
            Fecha = DateTime.UtcNow,
            TotalSoles = 0,
            TotalPuntos = puntosRequeridos,
            Estado = "pendiente"
        };

        _context.pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        var detalle = new detallepedido
        {
            PedidoId = pedido.Id,
            ProductoId = producto.Id,
            Cantidad = cantidad,
            SubtotalSoles = 0,
            SubtotalPuntos = puntosRequeridos
        };

        _context.detallepedidos.Add(detalle);
        producto.Stock -= cantidad;
        usuario.Puntos -= puntosRequeridos;

        await _context.SaveChangesAsync();
        return "Canje exitoso.";
    }
}

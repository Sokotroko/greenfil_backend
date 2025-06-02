using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace greenfil_backend.Models;

public partial class GreenfilContext : DbContext
{
    public GreenfilContext()
    {
    }

    public GreenfilContext(DbContextOptions<GreenfilContext> options)
        : base(options)
    {
    }

    public virtual DbSet<detallepedido> detallepedidos { get; set; }

    public virtual DbSet<modelo3d> modelo3ds { get; set; }

    public virtual DbSet<pedido> pedidos { get; set; }

    public virtual DbSet<producto> productos { get; set; }

    public virtual DbSet<usuario> usuarios { get; set; }

    public virtual DbSet<usuariosback> usuariosbacks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=1234;database=Greenfil", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.42-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<detallepedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.SubtotalPuntos).HasDefaultValueSql("'0'");
            entity.Property(e => e.SubtotalSoles).HasDefaultValueSql("'0.00'");

            entity.HasOne(d => d.Pedido).WithMany(p => p.detallepedidos).HasConstraintName("detallepedido_ibfk_1");

            entity.HasOne(d => d.Producto).WithMany(p => p.detallepedidos).HasConstraintName("detallepedido_ibfk_2");
        });

        modelBuilder.Entity<modelo3d>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Usuario).WithMany(p => p.modelo3ds).HasConstraintName("modelo3d_ibfk_1");
        });

        modelBuilder.Entity<pedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Estado).HasDefaultValueSql("'pendiente'");
            entity.Property(e => e.Fecha).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.TotalPuntos).HasDefaultValueSql("'0'");
            entity.Property(e => e.TotalSoles).HasDefaultValueSql("'0.00'");

            entity.HasOne(d => d.Usuario).WithMany(p => p.pedidos).HasConstraintName("pedido_ibfk_1");
        });

        modelBuilder.Entity<producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.PrecioPuntos).HasDefaultValueSql("'0'");
            entity.Property(e => e.PrecioSoles).HasDefaultValueSql("'0.00'");
            entity.Property(e => e.Stock).HasDefaultValueSql("'0'");
        });

        modelBuilder.Entity<usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Puntos).HasDefaultValueSql("'0'");
        });

        modelBuilder.Entity<usuariosback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Rol).HasDefaultValueSql("'admin'");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using SistemaVenta.Model.Models;

namespace SistemaVenta.DAL.DBContext;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<Detallecompra> Detallecompras { get; set; }

    public virtual DbSet<Detalleventa> Detalleventa { get; set; }

    public virtual DbSet<ImagenProducto> ImagenProductos { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Menurol> Menurols { get; set; }

    public virtual DbSet<Numerocompra> Numerocompras { get; set; }

    public virtual DbSet<Numeroventa> Numeroventa { get; set; }

    public virtual DbSet<Presentacion> Presentacions { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<ProductoEmpaque> ProductoEmpaques { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Venta { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PRIMARY");

            entity.ToTable("categoria");

            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.EsActivo)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("esActivo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra).HasName("PRIMARY");

            entity.ToTable("compra");

            entity.HasIndex(e => e.IdProveedor, "fk_proveedor1");

            entity.Property(e => e.IdCompra).HasColumnName("idCompra");
            entity.Property(e => e.FechaCompra)
                .HasColumnType("datetime")
                .HasColumnName("fechaCompra");
            entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");
            entity.Property(e => e.NumCompra)
                .HasMaxLength(40)
                .HasColumnName("numCompra");
            entity.Property(e => e.TipoPago)
                .HasMaxLength(50)
                .HasColumnName("tipoPago");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("fk_proveedor1");
        });

        modelBuilder.Entity<Detallecompra>(entity =>
        {
            entity.HasKey(e => e.IdDetalleCompra).HasName("PRIMARY");

            entity.ToTable("detallecompra");

            entity.HasIndex(e => e.IdCompra, "fk_compra");

            entity.HasIndex(e => e.IdProductoEmpaque, "fk_detalleCompra_productoEmpaque");

            entity.Property(e => e.IdDetalleCompra).HasColumnName("idDetalleCompra");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdCompra).HasColumnName("idCompra");
            entity.Property(e => e.IdProductoEmpaque).HasColumnName("idProductoEmpaque");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("precioUnitario");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 2)
                .HasColumnName("subtotal");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.Detallecompras)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("fk_compra");

            entity.HasOne(d => d.IdProductoEmpaqueNavigation).WithMany(p => p.Detallecompras)
                .HasForeignKey(d => d.IdProductoEmpaque)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalleCompra_productoEmpaque");
        });

        modelBuilder.Entity<Detalleventa>(entity =>
        {
            entity.HasKey(e => e.IdDetalleVenta).HasName("PRIMARY");

            entity.ToTable("detalleventa");

            entity.HasIndex(e => e.IdProductoEmpaque, "fk_detalleVenta_productoEmpaque");

            entity.HasIndex(e => e.IdVenta, "fk_detalleVenta_venta");

            entity.Property(e => e.IdDetalleVenta).HasColumnName("idDetalleVenta");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdProductoEmpaque).HasColumnName("idProductoEmpaque");
            entity.Property(e => e.IdVenta).HasColumnName("idVenta");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.IdProductoEmpaqueNavigation).WithMany(p => p.Detalleventa)
                .HasForeignKey(d => d.IdProductoEmpaque)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalleVenta_productoEmpaque");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.Detalleventa)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalleVenta_venta");
        });

        modelBuilder.Entity<ImagenProducto>(entity =>
        {
            entity.HasKey(e => e.IdImagen).HasName("PRIMARY");

            entity.ToTable("imagen_producto");

            entity.Property(e => e.IdImagen).HasColumnName("idImagen");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.NombreArchivo)
                .HasMaxLength(100)
                .HasColumnName("nombreArchivo");
            entity.Property(e => e.Ruta)
                .HasMaxLength(350)
                .HasColumnName("ruta");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.IdMenu).HasName("PRIMARY");

            entity.ToTable("menu");

            entity.HasIndex(e => e.IdPadre, "fk_menu_padre");

            entity.Property(e => e.IdMenu).HasColumnName("idMenu");
            entity.Property(e => e.Icono)
                .HasMaxLength(50)
                .HasColumnName("icono");
            entity.Property(e => e.IdPadre).HasColumnName("idPadre");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Url)
                .HasMaxLength(50)
                .HasColumnName("url");

            entity.HasOne(d => d.IdPadreNavigation).WithMany(p => p.InverseIdPadreNavigation)
                .HasForeignKey(d => d.IdPadre)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_menu_padre");
        });

        modelBuilder.Entity<Menurol>(entity =>
        {
            entity.HasKey(e => e.IdMenuRol).HasName("PRIMARY");

            entity.ToTable("menurol");

            entity.HasIndex(e => e.IdMenu, "idMenu");

            entity.HasIndex(e => e.IdRol, "idRol");

            entity.Property(e => e.IdMenuRol).HasColumnName("idMenuRol");
            entity.Property(e => e.IdMenu).HasColumnName("idMenu");
            entity.Property(e => e.IdRol).HasColumnName("idRol");

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.Menurols)
                .HasForeignKey(d => d.IdMenu)
                .HasConstraintName("menurol_ibfk_1");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Menurols)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("menurol_ibfk_2");
        });

        modelBuilder.Entity<Numerocompra>(entity =>
        {
            entity.HasKey(e => e.IdNumero).HasName("PRIMARY");

            entity.ToTable("numerocompra");

            entity.Property(e => e.IdNumero).HasColumnName("idNumero");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
        });

        modelBuilder.Entity<Numeroventa>(entity =>
        {
            entity.HasKey(e => e.IdNumero).HasName("PRIMARY");

            entity.ToTable("numeroventa");

            entity.Property(e => e.IdNumero).HasColumnName("idNumero");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
        });

        modelBuilder.Entity<Presentacion>(entity =>
        {
            entity.HasKey(e => e.IdPresentacion).HasName("PRIMARY");

            entity.ToTable("presentacion");

            entity.Property(e => e.IdPresentacion).HasColumnName("idPresentacion");
            entity.Property(e => e.CantidadUnidades).HasColumnName("cantidadUnidades");
            entity.Property(e => e.EsActivo)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("esActivo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PRIMARY");

            entity.ToTable("producto");

            entity.HasIndex(e => e.IdCategoria, "fk_categoria");

            entity.HasIndex(e => e.IdImagen, "fk_imagen");

            entity.HasIndex(e => e.IdProveedor, "fk_proveedor");

            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.EsActivo)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("esActivo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.IdImagen).HasColumnName("idImagen");
            entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("fk_categoria");

            entity.HasOne(d => d.IdImagenNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdImagen)
                .HasConstraintName("fk_imagen");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("fk_proveedor");
        });

        modelBuilder.Entity<ProductoEmpaque>(entity =>
        {
            entity.HasKey(e => e.IdProductoEmpaque).HasName("PRIMARY");

            entity.ToTable("producto_empaque");

            entity.HasIndex(e => e.IdPresentacion, "fk_presentacion_empaque");

            entity.HasIndex(e => e.IdProducto, "fk_producto");

            entity.Property(e => e.IdProductoEmpaque).HasColumnName("idProductoEmpaque");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.EsActivo)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("esActivo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdPresentacion).HasColumnName("idPresentacion");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.PrecioCompra)
                .HasPrecision(10, 2)
                .HasColumnName("precioCompra");
            entity.Property(e => e.PrecioVenta)
                .HasPrecision(10, 2)
                .HasColumnName("precioVenta");

            entity.HasOne(d => d.IdPresentacionNavigation).WithMany(p => p.ProductoEmpaques)
                .HasForeignKey(d => d.IdPresentacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_presentacion_empaque");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ProductoEmpaques)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_producto");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PRIMARY");

            entity.ToTable("proveedor");

            entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");
            entity.Property(e => e.Contacto)
                .HasMaxLength(50)
                .HasColumnName("contacto");
            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .HasColumnName("direccion");
            entity.Property(e => e.EsActivo)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("esActivo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PRIMARY");

            entity.ToTable("rol");

            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.IdRol, "idRol");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Clave)
                .HasMaxLength(150)
                .HasColumnName("clave");
            entity.Property(e => e.EsActivo)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("esActivo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(100)
                .HasColumnName("nombreCompleto");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("usuario_ibfk_1");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("PRIMARY");

            entity.ToTable("venta");

            entity.HasIndex(e => e.IdUsuario, "fk_usuarios");

            entity.Property(e => e.IdVenta).HasColumnName("idVenta");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.NumVenta)
                .HasMaxLength(40)
                .HasColumnName("numVenta");
            entity.Property(e => e.TipoPago)
                .HasMaxLength(50)
                .HasColumnName("tipoPago");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("fk_usuarios");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

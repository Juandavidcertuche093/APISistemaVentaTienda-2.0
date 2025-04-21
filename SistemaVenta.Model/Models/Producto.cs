using System;
using System.Collections.Generic;

namespace SistemaVenta.Model.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdCategoria { get; set; }

    public int? IdProveedor { get; set; }

    public int? IdImagen { get; set; }

    public int Stock { get; set; }

    public ulong? EsActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Categoria? IdCategoriaNavigation { get; set; }

    public virtual ImagenProducto? IdImagenNavigation { get; set; }

    public virtual Proveedor? IdProveedorNavigation { get; set; }

    public virtual ICollection<ProductoEmpaque> ProductoEmpaques { get; set; } = new List<ProductoEmpaque>();
}

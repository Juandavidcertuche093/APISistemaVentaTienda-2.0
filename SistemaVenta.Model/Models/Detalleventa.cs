using System;
using System.Collections.Generic;

namespace SistemaVenta.Model.Models;

public partial class Detalleventa
{
    public int IdDetalleVenta { get; set; }

    public int IdVenta { get; set; }

    public int IdProductoEmpaque { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Precio { get; set; }

    public decimal? Total { get; set; }

    public virtual ProductoEmpaque IdProductoEmpaqueNavigation { get; set; } = null!;

    public virtual Venta IdVentaNavigation { get; set; } = null!;
}

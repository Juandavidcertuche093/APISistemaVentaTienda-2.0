using System;
using System.Collections.Generic;

namespace SistemaVenta.Model.Models;

public partial class Detallecompra
{
    public int IdDetalleCompra { get; set; }

    public int? IdCompra { get; set; }

    public int IdProductoEmpaque { get; set; }

    public int? Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public decimal? Subtotal { get; set; }

    public virtual Compra? IdCompraNavigation { get; set; }

    public virtual ProductoEmpaque IdProductoEmpaqueNavigation { get; set; } = null!;
}

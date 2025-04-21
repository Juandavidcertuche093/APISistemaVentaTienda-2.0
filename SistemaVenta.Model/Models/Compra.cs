using System;
using System.Collections.Generic;

namespace SistemaVenta.Model.Models;

public partial class Compra
{
    public int IdCompra { get; set; }

    public int? IdProveedor { get; set; }

    public string NumCompra { get; set; } = null!;

    public DateTime? FechaCompra { get; set; }

    public decimal Total { get; set; }

    public string TipoPago { get; set; } = null!;

    public virtual ICollection<Detallecompra> Detallecompras { get; set; } = new List<Detallecompra>();

    public virtual Proveedor? IdProveedorNavigation { get; set; }
}

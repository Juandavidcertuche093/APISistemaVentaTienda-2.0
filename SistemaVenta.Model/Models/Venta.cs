using System;
using System.Collections.Generic;

namespace SistemaVenta.Model.Models;

public partial class Venta
{
    public int IdVenta { get; set; }

    public int? IdUsuario { get; set; }

    public string? NumVenta { get; set; } = null!;

    public string? TipoPago { get; set; } = null!;

    public decimal? Total { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Detalleventa> Detalleventa { get; set; } = new List<Detalleventa>();

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}

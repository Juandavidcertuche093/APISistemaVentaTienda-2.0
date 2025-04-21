using System;
using System.Collections.Generic;

namespace SistemaVenta.Model.Models;

public partial class Presentacion
{
    public int IdPresentacion { get; set; }

    public string Nombre { get; set; } = null!;

    public int CantidadUnidades { get; set; }

    public ulong? EsActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<ProductoEmpaque> ProductoEmpaques { get; set; } = new List<ProductoEmpaque>();
}

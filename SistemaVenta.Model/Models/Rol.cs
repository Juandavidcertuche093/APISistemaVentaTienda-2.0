using System;
using System.Collections.Generic;

namespace SistemaVenta.Model.Models;

public partial class Rol
{
    public int IdRol { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Menurol> Menurols { get; set; } = new List<Menurol>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}

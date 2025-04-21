using System;
using System.Collections.Generic;

namespace SistemaVenta.Model.Models;

public partial class Menu
{
    public int IdMenu { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Icono { get; set; }

    public string Url { get; set; } = null!;

    public int? IdPadre { get; set; }

    public virtual Menu? IdPadreNavigation { get; set; }

    public virtual ICollection<Menu> InverseIdPadreNavigation { get; set; } = new List<Menu>();

    public virtual ICollection<Menurol> Menurols { get; set; } = new List<Menurol>();
}

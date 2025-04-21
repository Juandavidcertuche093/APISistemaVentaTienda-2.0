using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class ProductoMasVendidoDTO
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
    }
}

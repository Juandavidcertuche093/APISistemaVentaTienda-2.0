using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class CompraDTO
    {
        public int IdCompra { get; set; }
        public string? NumCompra { get; set; }
        public int IdProveedor { get; set; }
        public string? NombreProveedor { get; set; }//Este campo se usa para mostrar el nombre del proveedor al que pertenece la compra
        public string? TipoPago { get; set; }
        public string? TotalTexto { get; set; }//cambiamos decimal por string
        public string? FechaCompra { get; set; }//cambiamos dataTime(fecha) por string
        public virtual ICollection<DetalleCompraDTO> DetalleCompras { get; set; } = new List<DetalleCompraDTO>();
    }
}

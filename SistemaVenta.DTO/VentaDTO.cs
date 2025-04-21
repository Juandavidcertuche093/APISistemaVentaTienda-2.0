using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class VentaDTO
    {
        public int IdVenta { get; set; }
        public string? NumVenta { get; set; }
        public int IdUsuario { get; set; }
        public string? UsuarioDescripcion { get; set; }//Este campo se usa para mostrar el nombre del Usuario al que pertenece la venta
        public string? TipoPago { get; set; }
        public string? TotalTexto { get; set; }//cambiamos decimal por string
        public string? FechaRegistro { get; set; }//cambiamos dataTime(fecha) por string
        public virtual ICollection<DetalleVentaDTO> Detalleventa { get; set; } = new List<DetalleVentaDTO>();
    }
}

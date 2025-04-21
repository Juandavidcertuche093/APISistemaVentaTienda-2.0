using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class ReporteDTO
    {
        public string NumeroVenta { get; set; }           // De la tabla Venta (numVenta)
        public string TipoPago { get; set; }              // De la tabla Venta
        public string FechaRegistro { get; set; }         // De la tabla Venta
        public string TotalVenta { get; set; }            // De la tabla Venta (total)
        public string NombreProducto { get; set; }        // De producto_empaque -> producto (nombre)
        public string Presentacion { get; set; }          // De producto_empaque -> presentacion (nombre)
        public int Cantidad { get; set; }                 // De detalleVenta (cantidad)
        public string Precio { get; set; }                // De detalleVenta (precio)
        public string Total { get; set; }                 // De detalleVenta (total)
        public string Usuario { get; set; }               // De venta -> usuario (nombreCompleto)
    }
}

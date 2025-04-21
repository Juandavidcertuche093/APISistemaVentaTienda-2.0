using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class VentaSemanaDTO
    {
        public string? Fecha { get; set; }// Fecha de la venta en formato string
        public int? Total { get; set; }// Número de ventas realizadas ese día.
    }
}

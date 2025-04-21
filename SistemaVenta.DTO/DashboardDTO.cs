using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class DashboardDTO
    {
        public int TotalVentas { get; set; } // Total de ventas de la última semana
        public string? TotalIngresos { get; set; }// Total de ingresos como string para darle formato monetario
        public int TotalProductos { get; set; } // Total de productos en stock
        public int TotalUsuarios { get; set; }//totla de usuarios NUEVO
        public List<VentaSemanaDTO> VentasUltimaSemana { get; set; } = new List<VentaSemanaDTO>();// Lista de ventas de los últimos 7 días
        public List<ProductoMasVendidoDTO> ProductosMasVendidos { get; set; } = new List<ProductoMasVendidoDTO>(); // 🔹 Nuevo campo
    }
}

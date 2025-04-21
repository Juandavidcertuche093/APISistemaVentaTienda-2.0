using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class ProveedorDTO
    {
        public int IdProveedor { get; set; }
        public string? Nombre { get; set; } = null!;
        public string? Direccion { get; set; } = null!;
        public string? Contacto { get; set; } = null!;
        public int? EsActivo { get; set; }//lo cambiamos de boolen a entero 
    }
}

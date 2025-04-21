using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class ProductoDTO
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = null!;
        public int? IdCategoria { get; set; }
        public string? DescripcionCategoria { get; set; } //específicamente para mostrar la descripción o nombre de la categoría
        public int? IdProveedor { get; set; }
        public string? NombreProveedor { get; set; } //específicamente para mostrar el nombre del proveedor
        public int? IdImagen { get; set; }
        public string? NombreImagen { get; set; } // Nombre del archivo de imagen
        public string? RutaImagen { get; set; } // Ruta de la imagen en el servidor
        public int Stock { get; set; }             
        public int? EsActivo { get; set; }//lo cambiamos de boolen a entero 
    }
}

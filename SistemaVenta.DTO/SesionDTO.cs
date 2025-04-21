using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class SesionDTO
    {
        public int IdUsuario { get; set; }
        public string? NombreCompleto { get; set; }
        public string? RolDescripcion { get; set; }//específicamente para mostrar la descripción o nombre de el Rol, que no está presente en el modelo Usuario 
        public string? Token { get; set; }
    }
}

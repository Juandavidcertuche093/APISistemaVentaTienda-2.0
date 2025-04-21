using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string? NombreCompleto { get; set; }
        public int? IdRol { get; set; }
        public string? RolDescripcion { get; set; }//Este campo se usa para mostrar el nombre del rol al que pertenece el usuario.
        public string? Clave { get; set; }
        public int? EsActivo { get; set; }//lo cambiamos de boolen a entero 
    }
}

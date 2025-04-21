using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class PresentacionDTO
    {
        public int IdPresentacion { get; set; }

        public string Nombre { get; set; } = null!;

        public int CantidadUnidades { get; set; }

        public int? EsActivo { get; set; }

        public virtual ICollection<ProductoEmpaqueDTO> ProductoEmpaques { get; set; } = new List<ProductoEmpaqueDTO>();
    }
}

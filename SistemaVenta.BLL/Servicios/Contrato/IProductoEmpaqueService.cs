using SistemaVenta.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios.Contrato
{
    public interface IProductoEmpaqueService
    {
        Task<List<ProductoEmpaqueDTO>> Lista();
        Task<ProductoEmpaqueDTO> Crear(ProductoEmpaqueDTO modelo);
        Task<bool> Editar(ProductoEmpaqueDTO modelo);
        Task<bool> Eliminar(int id);
    }
}

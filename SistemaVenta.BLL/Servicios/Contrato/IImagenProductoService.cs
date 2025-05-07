using SistemaVenta.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios.Contrato
{
    public interface IImagenProductoService
    {
        Task<List<ImagenProductoDTO>> Lista();
        Task<ImagenProductoDTO> Crear(ImagenProductoDTO modelo);
        Task<bool> Editar(ImagenProductoDTO modelo);
        Task<bool> Eliminar(int id);
    }
}

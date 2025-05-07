using SistemaVenta.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios.Contrato
{
    public interface ICompraService
    {
        Task<CompraDTO> Registrar(CompraDTO modelo);
        Task<List<CompraDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin);// esto nos ayudara para filtrar la ventas por numero de venta o por fechas

        //Task<List<ReporteDTO>> Reportes(string fechaInicio, string fechaFin);
    }
}

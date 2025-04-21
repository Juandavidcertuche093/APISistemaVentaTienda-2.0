using SistemaVenta.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DAL.Repositorio.Contrato
{
    public interface ICompraRepository : IGenericRepository<Compra>
    {
        Task<Compra> Registrar(Compra modelo);
    }
}

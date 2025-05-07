using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorio.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios
{
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _compraRepositorio;
        private readonly IGenericRepository<Detallecompra> _detalleCompraRepositorio;
        private readonly IMapper _mapper;

        public CompraService(ICompraRepository compraRepositorio, IGenericRepository<Detallecompra> detalleCompraRepositorio, IMapper mapper)
        {
            _compraRepositorio = compraRepositorio;
            _detalleCompraRepositorio = detalleCompraRepositorio;
            _mapper = mapper;
        }

        public async Task<CompraDTO> Registrar(CompraDTO modelo)
        {
            try
            {
                var compraGenerada = await _compraRepositorio.Registrar(_mapper.Map<Compra>(modelo));
                if (compraGenerada.IdCompra == 0)
                    throw new TaskCanceledException("No se pudo crear");
                return _mapper.Map<CompraDTO>(compraGenerada);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<CompraDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
        {
            IQueryable<Compra> query = await _compraRepositorio.Consultar();
            var listaResultado = new List<Compra>();

            try
            {
                //filtro para buscar por fecha y por numero de venta
                if (buscarPor == "fecha")
                {
                    DateTime fecha_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-CO"));
                    DateTime fecha_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-CO"));

                    listaResultado = await query.Where(v =>
                        v.FechaCompra.Value.Date >= fecha_Inicio.Date &&
                        v.FechaCompra.Value.Date <= fecha_Fin.Date
                    ).Include(c => c.IdProveedorNavigation)
                     .Include(dv => dv.Detallecompras)
                    .ThenInclude(p => p.IdProductoEmpaqueNavigation)
                    .ThenInclude(pe => pe.IdProductoNavigation)
                    .ToListAsync();
                }
                else
                {
                    listaResultado = await query.Where(v => v.NumCompra == numeroVenta
                    ).Include(c => c.IdProveedorNavigation)
                     .Include(dv => dv.Detallecompras)
                    .ThenInclude(p => p.IdProductoEmpaqueNavigation)

                    .ToListAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return _mapper.Map<List<CompraDTO>>(listaResultado);
        }
    }
}

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
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepositorio;
        private readonly IGenericRepository<Detalleventa> _detalleVentaRepositorio;
        private readonly IMapper _mapper;

        public VentaService(IVentaRepository ventaRepositorio, IGenericRepository<Detalleventa> detalleVentaRepositorio, IMapper mapper)
        {
            _ventaRepositorio = ventaRepositorio;
            _detalleVentaRepositorio = detalleVentaRepositorio;
            _mapper = mapper;
        }

        public async Task<VentaDTO> Registrar(VentaDTO modelo)
        {
            try
            {
                // Agregar la lógica para asignar el ID del usuario a la venta
                var venta = _mapper.Map<Venta>(modelo);
                venta.IdUsuario = modelo.IdUsuario;

                var ventaGenerada = await _ventaRepositorio.Registrar(_mapper.Map<Venta>(modelo));
                if (ventaGenerada.IdVenta == 0)
                    throw new TaskCanceledException("No se pudo crear");
                return _mapper.Map<VentaDTO>(ventaGenerada);
            }
            catch
            {

                throw;
            }
        }

        public async Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
        {

            IQueryable<Venta> query = await _ventaRepositorio.Consultar();
            var listaResultado = new List<Venta>();

            try
            {
                // filtro para buscar por fecha y por numero de venta
                if (buscarPor == "fecha")
                {
                    DateTime fecha_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-CO"));
                    DateTime fecha_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-CO"));

                    listaResultado = await query.Where(v =>
                        v.FechaRegistro.Value.Date >= fecha_Inicio.Date &&
                        v.FechaRegistro.Value.Date <= fecha_Fin.Date
                    ).Include(dv => dv.Detalleventa)
                    .ThenInclude(p => p.IdProductoEmpaqueNavigation)
                    .ThenInclude(pe => pe.IdProductoNavigation) // 👈 esto es clave
                    .Include(v => v.IdUsuarioNavigation)  // Incluimos el usuario aquí también
                    .ToListAsync();

                }
                else
                {
                    listaResultado = await query.Where(v => v.NumVenta == numeroVenta
                    ).Include(dv => dv.Detalleventa)
                    .ThenInclude(p => p.IdProductoEmpaqueNavigation)
                    .ThenInclude(pe => pe.IdProductoNavigation) // 👈 Añadir este
                    .Include(v => v.IdUsuarioNavigation)  // Incluimos el usuario
                    .ToListAsync();
                }
            }
            catch
            {

                throw;
            }
            return _mapper.Map<List<VentaDTO>>(listaResultado);
        }

        public async Task<List<ReporteDTO>> Reportes(string fechaInicio, string fechaFin)
        {

            IQueryable<Detalleventa> query = await _detalleVentaRepositorio.Consultar();
            var listaResultado = new List<Detalleventa>();

            try
            {
                DateTime fecha_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-CO"));
                DateTime fecha_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-CO"));

                listaResultado = await query
                    .Include(dv => dv.IdProductoEmpaqueNavigation)
                    .ThenInclude(pe => pe.IdProductoNavigation)
                    .Include(dv => dv.IdProductoEmpaqueNavigation)
                    .ThenInclude(pe => pe.IdPresentacionNavigation) // si tienes navegación a presentación
                    .Include(dv => dv.IdVentaNavigation)
                    .ThenInclude(v => v.IdUsuarioNavigation)
                    .Where(dv =>
                        dv.IdVentaNavigation.FechaRegistro.Value.Date >= fecha_Inicio.Date &&
                        dv.IdVentaNavigation.FechaRegistro.Value.Date <= fecha_Fin.Date
                    ).ToListAsync();

            }
            catch
            {

                throw;
            }

            return _mapper.Map<List<ReporteDTO>>(listaResultado);
        }
    }
}

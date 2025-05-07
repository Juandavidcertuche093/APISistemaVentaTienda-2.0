using AutoMapper;
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
    public class DashboardService : IDashboardService
    {
        private readonly IVentaRepository _ventaRepositorio;
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;
        private readonly IMapper _mapper;

        public DashboardService(IVentaRepository ventaRepositorio, IGenericRepository<Producto> productoRepositorio, IGenericRepository<Usuario> usuarioRepositorio, IMapper mapper)
        {
            _ventaRepositorio = ventaRepositorio;
            _productoRepositorio = productoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
        }

        //este metodo devuelve las ventas desde una fecha pasada hasta hoy
        private IQueryable<Venta> retornarVentas(IQueryable<Venta> tablaVenta, int diasAtras)
        {
            DateTime fechaLimite = DateTime.Today.AddDays(-diasAtras); // Fecha de hace X días
            return tablaVenta.Where(v => v.FechaRegistro.HasValue && v.FechaRegistro.Value.Date >= fechaLimite);
        }

        private async Task<List<ProductoMasVendidoDTO>> ProductoMasVendidos(IQueryable<Venta> ventas)
        {
            var ventasUltimaSemana = retornarVentas(ventas, 7)
                .SelectMany(v => v.Detalleventa)
                .GroupBy(dv => dv.IdProductoEmpaqueNavigation.IdProductoNavigation.IdProducto)
                .Select(g => new ProductoMasVendidoDTO
                {
                    IdProducto = g.Key,
                    Nombre = g.First().IdProductoEmpaqueNavigation.IdProductoNavigation.Nombre,
                    CantidadVendida = g.Sum(d =>
                        (d.Cantidad ?? 0) *
                        (d.IdProductoEmpaqueNavigation.IdPresentacionNavigation.CantidadUnidades)
                    )
                })
                .OrderByDescending(m => m.CantidadVendida)
                .Take(5)
                .ToList();

            return ventasUltimaSemana;
        }


        //calcula el total de las ventas en la ultima semana
        private async Task<int> TotalVenta1UltimaSemana(IQueryable<Venta> ventas)
        {
            return retornarVentas(ventas, 7).AsEnumerable().Count();
        }

        private async Task<string> TotalIngresosUltimaSemana(IQueryable<Venta> ventas)
        {
            decimal resultado = retornarVentas(ventas, 7)
                .AsEnumerable()
                .Sum(v => v.Total ?? 0); // Evita errores con valores null

            return resultado.ToString("N2", new CultureInfo("es-CO"));
        }


        //Suma los ingresos de la última semana y los devuelve como un string formateado
        private async Task<string> TotalIngresosUltimaSemana()
        {

            decimal resultado = 0;
            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();// Consulta todas las ventas

            if (_ventaQuery.Count() > 0)
            {
                var tablaventa = retornarVentas(_ventaQuery, -7);// Obtener las ventas de la última semana

                resultado = tablaventa.Select(v => v.Total).Sum(v => v.Value);// Sumar el total de ingresos
            }

            return Convert.ToString(resultado, new CultureInfo("es-CO"));// Convertir el resultado a string
        }


        //Devuelve el total de productos disponibles en stock.
        private async Task<int> TotalProductos()
        {

            IQueryable<Producto> _productoQuery = await _productoRepositorio.Consultar();// Consulta todos los productos

            int total = _productoQuery.Count();// Cuenta el total de productos
            return total;
        }


        //Devuelve las ventas agrupadas por fecha de los últimos 7 días.
        private async Task<List<VentaSemanaDTO>> VentasUltimaSemana(IQueryable<Venta> ventas)
        {
            return retornarVentas(ventas, 7)
                .AsEnumerable()
                .GroupBy(v => v.FechaRegistro.Value.Date)
                .OrderBy(g => g.Key)
                .Select(dv => new VentaSemanaDTO
                {
                    Fecha = dv.Key.ToString("dd/MM/yyyy"),
                    Total = dv.Count()
                })
                .ToList();
        }


        //Devuelve el total de suarios registrados.
        private async Task<int> TotalUsuarios()
        {

            IQueryable<Usuario> _usuarioQuery = await _usuarioRepositorio.Consultar();// Consulta todos los productos

            int total = _usuarioQuery.Count();// Cuenta el total de productos
            return total;
        }




        //Este método devuelve un resumen con todos los datos del dashboard.
        public async Task<DashboardDTO> Resumen()
        {
            var ventas = await _ventaRepositorio.Consultar();
            var productos = await _productoRepositorio.Consultar();
            var usuarios = await _usuarioRepositorio.Consultar();

            var vmDashBoard = new DashboardDTO();

            try
            {
                vmDashBoard.TotalVentas = await TotalVenta1UltimaSemana(ventas);
                vmDashBoard.TotalIngresos = await TotalIngresosUltimaSemana(ventas);
                vmDashBoard.TotalProductos = productos.Count();
                vmDashBoard.TotalUsuarios = usuarios.Count();
                vmDashBoard.VentasUltimaSemana = await VentasUltimaSemana(ventas);
                vmDashBoard.ProductosMasVendidos = await ProductoMasVendidos(ventas); // 🔹 Agregado aquí
            }
            catch (Exception ex)
            {
                // Mejor capturar y registrar el error en lugar de solo lanzar la excepción
                Console.WriteLine($"Error en Resumen(): {ex.Message}");
                throw;
            }

            return vmDashBoard;
        }

    }
}

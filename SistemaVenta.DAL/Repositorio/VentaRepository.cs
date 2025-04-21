using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorio.Contrato;
using SistemaVenta.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DAL.Repositorio
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        private readonly MyDbContext _dbContext;

        public VentaRepository(MyDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Venta> Registrar(Venta modelo)
        {
            Venta ventaGenerada = new Venta();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (Detalleventa dv in modelo.Detalleventa)
                    {
                        var productoEmpaque = _dbContext.ProductoEmpaques
                            .FirstOrDefault(pe => pe.IdProductoEmpaque == dv.IdProductoEmpaque);

                        if (productoEmpaque == null)
                        {
                            throw new Exception("Producto no encontrado.");
                        }

                        // Buscar el producto real en la tabla Producto
                        var producto = _dbContext.Productos
                            .FirstOrDefault(p => p.IdProducto == productoEmpaque.IdProducto);

                        if (producto == null)
                        {
                            throw new Exception("Producto en inventario no encontrado.");
                        }

                        // Obtener la cantidad de unidades por empaque desde la tabla Presentacion
                        int unidadesPorEmpaque = _dbContext.Presentacions
                            .Where(p => p.IdPresentacion == productoEmpaque.IdPresentacion)
                            .Select(p => (int?)p.CantidadUnidades) // Convierte a nullable int
                            .FirstOrDefault() ?? 1; // Si es null, usa 1 como valor por defecto

                        // Calcular la cantidad real de unidades a descontar
                        int unidadesADescontar = (dv.Cantidad ?? 0) * unidadesPorEmpaque;

                        if (producto.Stock < unidadesADescontar)
                        {
                            throw new Exception("Stock insuficiente para la venta.");
                        }

                        // Descontar del stock real del producto
                        producto.Stock -= unidadesADescontar;
                        _dbContext.Productos.Update(producto);
                    }

                    await _dbContext.SaveChangesAsync();

                    // Generar número de venta
                    Numeroventa correlativo = _dbContext.Numeroventa.First();
                    correlativo.UltimoNumero++;
                    correlativo.FechaRegistro = DateTime.Now;
                    _dbContext.Numeroventa.Update(correlativo);
                    await _dbContext.SaveChangesAsync();

                    modelo.NumVenta = correlativo.UltimoNumero.ToString("D4");
                    await _dbContext.Venta.AddAsync(modelo);
                    await _dbContext.SaveChangesAsync();

                    ventaGenerada = modelo;
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return ventaGenerada;
        }
    }
}

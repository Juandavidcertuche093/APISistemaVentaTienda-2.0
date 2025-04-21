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
    public class CompraRepository : GenericRepository<Compra>, ICompraRepository
    {
        private readonly MyDbContext _dbContext;

        public CompraRepository(MyDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Compra> Registrar(Compra modelo)
        {
            Compra compraGenerada = new Compra();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (Detallecompra dc in modelo.Detallecompras)
                    {
                        var productoEmpaque = _dbContext.ProductoEmpaques
                            .FirstOrDefault(pe => pe.IdProductoEmpaque == dc.IdProductoEmpaque);

                        if (productoEmpaque == null)
                        {
                            throw new Exception("Producto empaque no encontrado.");
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
                            .Select(p => (int?)p.CantidadUnidades)
                            .FirstOrDefault() ?? 1;

                        // Calcular la cantidad real de unidades a agregar
                        int unidadesAAgregar = (dc.Cantidad ?? 0) * unidadesPorEmpaque;

                        // Sumar al stock real del producto
                        producto.Stock += unidadesAAgregar;
                        _dbContext.Productos.Update(producto);
                    }

                    await _dbContext.SaveChangesAsync();

                    // Generar número de compra (similar a NumVenta)
                    var correlativoCompra = _dbContext.Numerocompras.FirstOrDefault();

                    if (correlativoCompra == null)
                    {
                        correlativoCompra = new Numerocompra
                        {
                            UltimoNumero = 1,
                            FechaRegistro = DateTime.Now
                        };
                        await _dbContext.Numerocompras.AddAsync(correlativoCompra);
                    }
                    else
                    {
                        correlativoCompra.UltimoNumero++;
                        correlativoCompra.FechaRegistro = DateTime.Now;
                        _dbContext.Numerocompras.Update(correlativoCompra);
                    }

                    await _dbContext.SaveChangesAsync();

                    modelo.NumCompra = correlativoCompra.UltimoNumero.ToString("D4");
                    modelo.FechaCompra = DateTime.Now;

                    await _dbContext.Compras.AddAsync(modelo);
                    await _dbContext.SaveChangesAsync();

                    compraGenerada = modelo;
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return compraGenerada;
        }
    }
}

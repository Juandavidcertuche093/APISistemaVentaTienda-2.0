using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorio.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios
{
    public class ProductoService : IProductoService
    {
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IMapper _mapper;       
        private const int StockMinimo = 10; // Definir el umbral como constante


        public ProductoService(IGenericRepository<Producto> productoRepositorio, IMapper mapper)
        {
            _productoRepositorio = productoRepositorio;
            _mapper = mapper;            
        }

        public async Task<List<ProductoDTO>> ObtenerProductosConStockBajo(int stockMinimo)
        {
            try
            {
                var producto = await _productoRepositorio.Consultar();
                var productosConStockBajo = producto
                    .Where(m => m.Stock <= stockMinimo)
                    .Include(cat => cat.IdCategoriaNavigation)
                    .ToList();

                return _mapper.Map<List<ProductoDTO>>(productosConStockBajo);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<ProductoDTO>> Lista()
        {
            try
            {
                var queryMedicamento = await _productoRepositorio.Consultar();

                var listaProducto = queryMedicamento
                    .Include(cat => cat.IdCategoriaNavigation)//incluimos la relacion con categoria del producto
                    .Include(prov => prov.IdProveedorNavigation)//incluimos la relacion con proveedor del producto
                    .Include(imag => imag.IdImagenNavigation)//incluimos la relacion con imagen del producto
                    .ToList();
                return _mapper.Map<List<ProductoDTO>>(listaProducto.ToList());
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProductoDTO> Crear(ProductoDTO modelo)
        {
            try
            {
                // Verificar si ya existe un producto con el mismo nombre
                var existeProducto = await _productoRepositorio.Obtener(m => m.Nombre.ToLower() == modelo.Nombre.ToLower());
                if (existeProducto != null)
                {
                    throw new Exception("Ya existe un Producto con este nombre.");
                }

                var productoEntidad = _mapper.Map<Producto>(modelo);
                var productoCreado = await _productoRepositorio.Crear(productoEntidad);

                if (productoCreado.IdProducto == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el Producto.");
                }

                // ✅ CORRECCIÓN: Usar 'await' para obtener la consulta antes de aplicar filtros
                var consulta = await _productoRepositorio.Consultar();
                var productoConRelaciones = consulta
                    .Where(p => p.IdProducto == productoCreado.IdProducto)
                    .Include(p => p.IdCategoriaNavigation)
                    .Include(p => p.IdProveedorNavigation)
                    .Include(p => p.IdImagenNavigation)
                    .FirstOrDefault(); // No se usa async porque ya cargamos la consulta en memoria

                return _mapper.Map<ProductoDTO>(productoConRelaciones);
            }
            catch
            {
                throw;
            }
        }



        public async Task<bool> Editar(ProductoDTO modelo)
        {
            try
            {
                var productoModelo = _mapper.Map<Producto>(modelo);//mapeo inverso convierte el DTO a medicamento para almacenar
                var productoEncotrado = await _productoRepositorio.Obtener(u => u.IdProducto == productoModelo.IdProducto);//buscamos el medicamento con su idmedicamento

                if (productoEncotrado == null)//si el medicamento no se encuentra en la base de datos se lanza una acepcion 
                    throw new TaskCanceledException("El producto no existe");
                productoEncotrado.Nombre = productoModelo.Nombre;
                productoEncotrado.IdCategoria = productoModelo.IdCategoria;
                productoEncotrado.IdImagen = productoModelo.IdImagen;
                productoEncotrado.Stock = productoModelo.Stock;                
                productoEncotrado.EsActivo = productoModelo.EsActivo;


                //Aquí se llama al método Editar del repositorio para persistir los cambios en la base de datos
                var productoActualizado = await _productoRepositorio.Editar(productoEncotrado);

                if (productoEncotrado.Stock <= 5)
                {
                    var productoDTO = _mapper.Map<ProductoDTO>(productoEncotrado);                    
                }

                // Verificar si el objeto devuelto no es nulo, lo que indica que la actualización fue exitosa (true)
                return productoActualizado != null;

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var productoEncotrado = await _productoRepositorio.Obtener(p => p.IdProducto == id);//buscamos el medicamto por su idmedicamento
                if (productoEncotrado == null)
                    throw new TaskCanceledException("El producto no existe");
                var productoEliminado = await _productoRepositorio.Eliminar(productoEncotrado);//Si lo encuentra, lo elimina usando el repositorio.
                return productoEliminado != null;//Devuelve true si la eliminación fue exitosa.
            }
            catch
            {
                throw;
            }
        }
    }
}

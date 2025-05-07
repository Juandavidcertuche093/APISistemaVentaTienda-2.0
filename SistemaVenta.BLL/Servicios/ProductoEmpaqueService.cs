using AutoMapper;
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
    public class ProductoEmpaqueService : IProductoEmpaqueService
    {
        private readonly IGenericRepository<ProductoEmpaque> _productoEmpaqueRepository;
        private readonly IMapper _mapper;

        public ProductoEmpaqueService(IGenericRepository<ProductoEmpaque> productoEmpaqueRepository, IMapper mapper)
        {
            _productoEmpaqueRepository = productoEmpaqueRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductoEmpaqueDTO>> Lista()
        {
            try
            {
                var queryProductosEmpaque = await _productoEmpaqueRepository.Consultar();

                var listaProductoEmpaque = queryProductosEmpaque
                    .Include(cat => cat.IdProductoNavigation)
                    .Include(pov => pov.IdPresentacionNavigation)
                    .ToList();
                return _mapper.Map<List<ProductoEmpaqueDTO>>(listaProductoEmpaque.ToList());

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ProductoEmpaqueDTO> Crear(ProductoEmpaqueDTO modelo)
        {
            try
            {
                var productoEntidad = _mapper.Map<ProductoEmpaque>(modelo);
                var productoCreado = await _productoEmpaqueRepository.Crear(productoEntidad);

                if (productoCreado.IdProducto == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el Producto.");
                }

                // ✅ CORRECCIÓN: Usar 'await' para obtener la consulta antes de aplicar filtros
                var consulta = await _productoEmpaqueRepository.Consultar();
                var productoConRelaciones = consulta
                    .Where(p => p.IdProducto == productoCreado.IdProducto)
                    .Include(p => p.IdPresentacionNavigation)
                    .FirstOrDefault(); // No se usa async porque ya cargamos la consulta en memoria

                return _mapper.Map<ProductoEmpaqueDTO>(productoConRelaciones);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Editar(ProductoEmpaqueDTO modelo)
        {
            try
            {
                var productoEmpaqueModelo = _mapper.Map<ProductoEmpaque>(modelo);
                var productoEnpaqueEcontrado = await _productoEmpaqueRepository.Obtener(u => u.IdProductoEmpaque == productoEmpaqueModelo.IdProductoEmpaque);

                if (productoEnpaqueEcontrado == null)
                    throw new TaskCanceledException("El producto no exite");
                productoEnpaqueEcontrado.IdProducto = productoEmpaqueModelo.IdProducto;
                productoEnpaqueEcontrado.IdPresentacion = productoEmpaqueModelo.IdPresentacion;
                productoEnpaqueEcontrado.Cantidad = productoEmpaqueModelo.Cantidad;
                productoEnpaqueEcontrado.PrecioVenta = productoEmpaqueModelo.PrecioVenta;
                productoEnpaqueEcontrado.PrecioCompra = productoEmpaqueModelo.PrecioCompra;
                productoEnpaqueEcontrado.EsActivo = productoEmpaqueModelo.EsActivo;

                var productoEnpaqueActualizado = await _productoEmpaqueRepository.Editar(productoEnpaqueEcontrado);

                return productoEnpaqueActualizado != null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var productoEnpaqueEcontrado = await _productoEmpaqueRepository.Obtener(p => p.IdProductoEmpaque == id);
                if (productoEnpaqueEcontrado == null)
                    throw new TaskCanceledException("El producto no exite");
                var productoEmpaqueEliminado = await _productoEmpaqueRepository.Eliminar(productoEnpaqueEcontrado);
                return productoEmpaqueEliminado != null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

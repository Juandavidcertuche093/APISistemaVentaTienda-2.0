using AutoMapper;
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
    public class ProveedorService : IProveedorService
    {
        private readonly IGenericRepository<Proveedor> _proveedorRepository;
        private readonly IMapper _mapper;

        public ProveedorService(IGenericRepository<Proveedor> proveedorRepository, IMapper mapper)
        {
            _proveedorRepository = proveedorRepository;
            _mapper = mapper;
        }

        public async Task<List<ProveedorDTO>> Lista()
        {
            try
            {
                var listaProveedor = await _proveedorRepository.Consultar();
                return _mapper.Map<List<ProveedorDTO>>(listaProveedor);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ProveedorDTO> Crear(ProveedorDTO modelo)
        {
            try
            {
                var proveedorCreado = await _proveedorRepository.Crear(_mapper.Map<Proveedor>(modelo));
                if (proveedorCreado.IdProveedor == 0)
                    throw new TaskCanceledException("No se pudo crear el proveedor");
                return _mapper.Map<ProveedorDTO>(proveedorCreado);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Editar(ProveedorDTO modelo)
        {
            try
            {
                var proveedorModelo = _mapper.Map<Proveedor>(modelo);
                var proveedorEncontrado = await _proveedorRepository.Obtener(u => u.IdProveedor == proveedorModelo.IdProveedor);

                if (proveedorEncontrado == null)
                    throw new TaskCanceledException("Proveedor no encontrado");
                proveedorEncontrado.Nombre = proveedorModelo.Nombre;
                proveedorEncontrado.Direccion = proveedorModelo.Direccion;
                proveedorEncontrado.Contacto = proveedorModelo.Contacto;
                proveedorEncontrado.EsActivo = proveedorModelo.EsActivo;

                var proveedorActualizado = await _proveedorRepository.Editar(proveedorEncontrado);

                return proveedorActualizado != null;
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
                var proveedorEncontrado = await _proveedorRepository.Obtener(u => u.IdProveedor == id);
                if (proveedorEncontrado == null)
                    throw new TaskCanceledException("Proveedor no encontrado");
                var proveedorEliminado = await _proveedorRepository.Eliminar(proveedorEncontrado);
                return proveedorEliminado != null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

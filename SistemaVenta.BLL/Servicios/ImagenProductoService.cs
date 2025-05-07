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
    public class ImagenProductoService : IImagenProductoService
    {
        private readonly IGenericRepository<ImagenProducto> _imagenProductoRepository;
        private readonly IMapper _mapper;

        public ImagenProductoService(IGenericRepository<ImagenProducto> imagenProductoRepository, IMapper mapper)
        {
            _imagenProductoRepository = imagenProductoRepository;
            _mapper = mapper;
        }
        public async Task<List<ImagenProductoDTO>> Lista()
        {
            try
            {
                var listaImagenProducto = await _imagenProductoRepository.Consultar();
                return _mapper.Map<List<ImagenProductoDTO>>(listaImagenProducto.ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ImagenProductoDTO> Crear(ImagenProductoDTO modelo)
        {
            try
            {
                var imagenProductoCreado = await _imagenProductoRepository.Crear(_mapper.Map<ImagenProducto>(modelo));//mapeo inverso convierte el DTO a categoria para almacenar
                if (imagenProductoCreado.IdImagen == 0)
                    throw new TaskCanceledException("No se pudo crear la categoría");
                return _mapper.Map<ImagenProductoDTO>(imagenProductoCreado);//devuelve el objeto creado en formato CategoriaDTO
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Editar(ImagenProductoDTO modelo)
        {
            try
            {
                var imagenModelo = _mapper.Map<ImagenProducto>(modelo);
                var imagenEncontrado = await _imagenProductoRepository.Obtener(u => u.IdImagen == imagenModelo.IdImagen);

                if (imagenEncontrado == null)
                    throw new TaskCanceledException("imagen no encontrado");
                imagenEncontrado.NombreArchivo = imagenModelo.NombreArchivo;
                imagenEncontrado.Ruta = imagenModelo.Ruta;

                var imagenActualizado = await _imagenProductoRepository.Editar(imagenEncontrado);

                return imagenActualizado != null;
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
                var imagenEncontrado = await _imagenProductoRepository.Obtener(u => u.IdImagen == id);
                if (imagenEncontrado == null)
                    throw new TaskCanceledException("iamgen no encontrado");
                var imagenEliminado = await _imagenProductoRepository.Eliminar(imagenEncontrado);
                return imagenEliminado != null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

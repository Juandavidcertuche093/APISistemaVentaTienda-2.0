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
    public class CategoriaService : ICategoriaService
    {
        private readonly IGenericRepository<Categoria> _categoriaRepository;
        private readonly IMapper _mapper;

        public CategoriaService(IGenericRepository<Categoria> categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoriaDTO>> Lista()
        {
            try
            {
                var listaCategoria = await _categoriaRepository.Consultar();
                return _mapper.Map<List<CategoriaDTO>>(listaCategoria.ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CategoriaDTO> Crear(CategoriaDTO modelo)
        {
            try
            {
                var categoriaCreado = await _categoriaRepository.Crear(_mapper.Map<Categoria>(modelo));//mapeo inverso convierte el DTO a categoria para almacenar
                if (categoriaCreado.IdCategoria == 0)
                    throw new TaskCanceledException("No se pudo crear la categoría");
                return _mapper.Map<CategoriaDTO>(categoriaCreado);//devuelve el objeto creado en formato CategoriaDTO
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Editar(CategoriaDTO modelo)
        {
            try
            {
                var categoriaModelo = _mapper.Map<Categoria>(modelo);
                var categoriaEncontrado = await _categoriaRepository.Obtener(u => u.IdCategoria == categoriaModelo.IdCategoria);

                if (categoriaEncontrado == null)
                    throw new TaskCanceledException("No se encontró la categoría");
                categoriaEncontrado.Nombre = categoriaModelo.Nombre;
                categoriaEncontrado.EsActivo = categoriaModelo.EsActivo;

                var categoriaActualizado = await _categoriaRepository.Editar(categoriaEncontrado);

                return categoriaActualizado != null;

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
                var categoriaEncontrada = await _categoriaRepository.Obtener(p => p.IdCategoria == id);//buscamos la categoria por su idCategoria
                if (categoriaEncontrada == null)
                    throw new TaskCanceledException("No se encontró la categoría");
                var categoriaEliminada = await _categoriaRepository.Eliminar(categoriaEncontrada);//si lo encuentra, lo elimina usando el repositorio
                return categoriaEliminada != null;//devuelve true si se elimina correctamente
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

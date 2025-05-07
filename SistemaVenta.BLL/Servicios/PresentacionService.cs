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
    public class PresentacionService : IPresentacionService
    {
        private readonly IGenericRepository<Presentacion> _presentacionRepository;
        private readonly IMapper _mapper;

        public PresentacionService(IGenericRepository<Presentacion> presentacionRepository, IMapper mapper)
        {
            _presentacionRepository = presentacionRepository;
            _mapper = mapper;
        }

        public async Task<List<PresentacionDTO>> Lista()
        {
            try
            {
                var listaPresentacion = await _presentacionRepository.Consultar();
                return _mapper.Map<List<PresentacionDTO>>(listaPresentacion.ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

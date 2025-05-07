using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utilidad;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PresentacionController : ControllerBase
    {
        private readonly IPresentacionService _presentacionService;

        public PresentacionController(IPresentacionService presentacionService)
        {
            _presentacionService = presentacionService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var rsp = new Response<List<PresentacionDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await _presentacionService.Lista();
            }
            catch (Exception)
            {

                rsp.status = false;
                rsp.msg = "Error al listar presentaciones";
            }
            return Ok(rsp);
        }
    }
}

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
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dahboardServicio;

        public DashboardController(IDashboardService dahboardServicio)
        {
            _dahboardServicio = dahboardServicio;
        }

        //devolvemos la lista de los productos
        [HttpGet]
        [Route("Resumen")]
        public async Task<IActionResult> Resumen()
        {
            var rsp = new Response<DashboardDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _dahboardServicio.Resumen();// Llama al servicio para obtener el resumen
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;// Maneja errores

            }
            return Ok(rsp);
        }
    }
}

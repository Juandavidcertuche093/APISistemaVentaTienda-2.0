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
    public class ProductoEmpaqueController : ControllerBase
    {
        private readonly IProductoEmpaqueService _productoEmpaque;

        public ProductoEmpaqueController(IProductoEmpaqueService productoEmpaque)
        {
            _productoEmpaque = productoEmpaque;
        }

        //devolvemos la lista de los roles
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var rsp = new Response<List<ProductoEmpaqueDTO>>();//lo que nos duelve una lista de RolDTO

            try
            {
                rsp.status = true;
                rsp.value = await _productoEmpaque.Lista();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;

            }
            return Ok(rsp);
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] ProductoEmpaqueDTO modelo)
        {
            var rsp = new Response<ProductoEmpaqueDTO>();

            try
            {
                if (modelo == null)
                {
                    rsp.status = false;
                    rsp.msg = "El modelo no puede ser nulo.";
                    return BadRequest(rsp);
                }

                rsp.status = true;
                rsp.value = await _productoEmpaque.Crear(modelo);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] ProductoEmpaqueDTO producto)// El objeto MedicamentoDTO se recibe en el cuerpo de la solicitud.
        {

            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _productoEmpaque.Editar(producto);//Llama al servicio para actualizar la información de un medicamento existente.

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;

            }
            return Ok(rsp);

        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {

            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _productoEmpaque.Eliminar(id);//Llama al servicio para eliminar un medicamento por su ID

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;

            }
            return Ok(rsp);

        }
    }
}

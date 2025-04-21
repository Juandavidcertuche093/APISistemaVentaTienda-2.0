using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utilidad;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;
using System.Security.Claims;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        //devolvemos la lista de los usurios
        [HttpGet]
        //[Authorize]//solo puede ver con el token
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var rsp = new Response<List<UsuarioDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await _usuarioService.Lista();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;

            }
            return Ok(rsp);
        }


        [HttpGet]
        //[Authorize]
        [Route("VerificarSesion")]
        public async Task<IActionResult> VerificarSesion()
        {
            var rsp = new Response<SesionDTO>();

            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                if (identity == null || !identity.Claims.Any())
                    throw new UnauthorizedAccessException("Token inválido o sesión expirada.");

                // Cambiamos para obtener el nombre completo en lugar del correo
                var nombreCompleto = identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

                if (string.IsNullOrEmpty(nombreCompleto))
                    throw new UnauthorizedAccessException("No se pudo obtener el usuario del token.");

                var usuarios = await _usuarioService.Lista();
                var usuario = usuarios.FirstOrDefault(u => u.NombreCompleto == nombreCompleto);

                if (usuario == null)
                    throw new UnauthorizedAccessException("Usuario no encontrado.");

                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                var sesion = new SesionDTO
                {
                    IdUsuario = usuario.IdUsuario,
                    NombreCompleto = usuario.NombreCompleto,
                    RolDescripcion = usuario.RolDescripcion,
                    Token = token,
                };

                rsp.status = true;
                rsp.value = sesion;
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }



        [HttpPost]
        [Route("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO login)
        {
            var rsp = new Response<SesionDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _usuarioService.ValidarCredenciales(login.Nombre, login.Clave);
            }
            catch (TaskCanceledException ex)
            {
                // Verifica si el mensaje de error es "Usuario inactivo, no puede iniciar sesión"
                if (ex.Message == "Usuario inactivo, no puede iniciar sesión")
                {
                    return StatusCode(403, new { msg = "Usuario inactivo, no puede iniciar sesión" });
                }
                else
                {
                    rsp.status = false;
                    rsp.msg = ex.Message;
                    return BadRequest(rsp);
                }
            }
            catch (Exception ex)
            {
                //rsp.status = false;
                //rsp.msg = "Error en el servidor";
                //return StatusCode(500, rsp);
            }

            return Ok(rsp);
        }


        [HttpPost]
        //[Authorize]//solo puede ver con el token
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] UsuarioDTO usuario)
        {

            var rsp = new Response<UsuarioDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _usuarioService.Crear(usuario);

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;

            }
            return Ok(rsp);

        }


        [HttpPut]
        //[Authorize]//solo puede ver con el token
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] UsuarioDTO usuario)
        {

            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _usuarioService.Editar(usuario);

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;

            }
            return Ok(rsp);

        }


        [HttpPut]
        //[Authorize]
        [Route("ActualizarEstado/{id:int}")]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody] ActualizarEstadoRequestDTO request)
        {
            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _usuarioService.ActualizarEstado(id, request.EsActivo);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }


        [HttpDelete]
        //[Authorize]//solo puede ver con el token
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {

            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _usuarioService.Eliminar(id);

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

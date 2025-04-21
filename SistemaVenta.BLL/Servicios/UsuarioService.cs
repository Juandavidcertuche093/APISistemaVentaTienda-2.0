using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorio.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model.Models;
using SistemaVenta.Utility.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _UsuarioRepositorio;
        private readonly IMapper _mapper;
        private readonly jwtBearer _jwtBearer;

        public UsuarioService(IGenericRepository<Usuario> usuarioRepositorio, IMapper mapper, jwtBearer jwtBearer)
        {
            _UsuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
            _jwtBearer = jwtBearer;
        }

        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                var queryUsuario = await _UsuarioRepositorio.Consultar();
                var listaUsuario = queryUsuario.Include(rol => rol.IdRolNavigation).ToList();//usando Include para cargar la relación entre usuario y rol, ToList(): Convierte la consulta en una lista.
                return _mapper.Map<List<UsuarioDTO>>(listaUsuario);
            }
            catch
            {

                throw;
            }
        }


        public async Task<SesionDTO> ValidarCredenciales(string nombreCompleto, string clave)
        {
            try
            {
                var queryUsuario = await _UsuarioRepositorio.Consultar(u => u.NombreCompleto == nombreCompleto);
                Usuario usuario = queryUsuario.FirstOrDefault();

                if (usuario == null)
                    throw new TaskCanceledException("El usuario no existe");

                // Verificar si el usuario está activo
                if (usuario.EsActivo == 0) // Si el campo es 0, el usuario está inactivo
                    throw new TaskCanceledException("Usuario inactivo, no puede iniciar sesión");

                bool esClaveValida = _jwtBearer.VerificarClave(clave, usuario.Clave);
                if (!esClaveValida)
                    throw new TaskCanceledException("Credenciales incorrectas");

                Usuario devolverUsuario = queryUsuario.Include(rol => rol.IdRolNavigation).First();
                string tokenJWT = _jwtBearer.generarJWT(devolverUsuario);

                SesionDTO sesion = _mapper.Map<SesionDTO>(devolverUsuario);
                sesion.Token = tokenJWT;

                return sesion;
            }
            catch
            {
                throw;
            }
        }


        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try
            {
                // Encriptar la contraseña usando bcrypt antes de guardar
                modelo.Clave = _jwtBearer.EncriptarConBcrypt(modelo.Clave);

                var usuarioCreado = await _UsuarioRepositorio.Crear(_mapper.Map<Usuario>(modelo));

                if (usuarioCreado.IdUsuario == 0)
                    throw new TaskCanceledException("No se pudo crear");

                var query = await _UsuarioRepositorio.Consultar(u => u.IdUsuario == usuarioCreado.IdUsuario);

                usuarioCreado = query.Include(rol => rol.IdRolNavigation).First();

                return _mapper.Map<UsuarioDTO>(usuarioCreado);
            }
            catch
            {

                throw;
            }
        }



        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(modelo);
                var usurioEncontrado = await _UsuarioRepositorio.Obtener(u => u.IdUsuario == usuarioModelo.IdUsuario);

                if (usurioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                usurioEncontrado.NombreCompleto = usuarioModelo.NombreCompleto;
                //usurioEncontrado.Correo = usuarioModelo.Correo;
                usurioEncontrado.IdRol = usuarioModelo.IdRol;

                // Si se está cambiando la contraseña, encriptarla
                if (!string.IsNullOrEmpty(usuarioModelo.Clave))
                {
                    usurioEncontrado.Clave = _jwtBearer.EncriptarConBcrypt(usuarioModelo.Clave);
                }

                usurioEncontrado.EsActivo = usuarioModelo.EsActivo;

                var usuarioActualizado = await _UsuarioRepositorio.Editar(usurioEncontrado);

                return usuarioActualizado != null;
            }
            catch
            {

                throw;
            }
        }


        public async Task<bool> ActualizarEstado(int id, int esActivo)
        {
            try
            {
                var usuarioEncontrado = await _UsuarioRepositorio.Obtener(u => u.IdUsuario == id);

                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                // Convertir explícitamente el valor de esActivo (int) a ulong?
                usuarioEncontrado.EsActivo = (ulong?)esActivo;

                var usuarioActualizado = await _UsuarioRepositorio.Editar(usuarioEncontrado);

                return usuarioActualizado != null;
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
                var usuarioEncontrado = await _UsuarioRepositorio.Obtener(u => u.IdUsuario == id);

                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("El usurio no exixte");

                var usurioEliminado = await _UsuarioRepositorio.Eliminar(usuarioEncontrado);

                return usurioEliminado != null;
            }
            catch
            {

                throw;
            }
        }
    }
}

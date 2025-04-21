using SistemaVenta.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios.Contrato
{
    public interface IUsuarioService
    {
        //interface para el usuario
        Task<List<UsuarioDTO>> Lista();
        Task<SesionDTO> ValidarCredenciales(string nombreCompleto, string clave);//Recibe un correo y una clave, valida las credenciales del usuario y genera un token JWT si son correctas.
        Task<UsuarioDTO> Crear(UsuarioDTO modelo);//Crea un nuevo usuario en el sistema, encriptando la contraseña.
        Task<bool> Editar(UsuarioDTO modelo);//Edita los datos de un usuario ya existente.
        Task<bool> ActualizarEstado(int id, int esActivo); // Nuevo método
        Task<bool> Eliminar(int id);//Elimina un usuario del sistema basado en su ID.
    }
}

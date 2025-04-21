using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SistemaVenta.Model.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.Utility.Authentication
{
    public class jwtBearer
    {
        private readonly IConfiguration _configuration;

        public jwtBearer(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // Método para encriptar una contraseña usando bcrypt
        public string EncriptarConBcrypt(string clave)
        {
            // Genera el hash de la contraseña usando bcrypt
            return BCrypt.Net.BCrypt.HashPassword(clave);
        }

        // Método para verificar una contraseña contra su hash
        public bool VerificarClave(string clave, string hashedClave)
        {
            // Verifica que la contraseña proporcionada coincida con el hash almacenado
            return BCrypt.Net.BCrypt.Verify(clave, hashedClave);
        }

        //Generar los jwt
        public string generarJWT(Usuario model)
        {
            //Claims: Son declaraciones sobre el usuario que se almacenan en el token. En este caso, se incluyen: idUsuario, nombreCompleto
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, model.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, model.NombreCompleto!)
            };

            //llaves de seguriada y algunas credenciales para wjt
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //crear el detalle del token
            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddHours(7),//7 horas dura el token AddHours
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}

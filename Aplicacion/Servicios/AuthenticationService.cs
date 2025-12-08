using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Exceptions;
using Aplicacion.Interfaces.AplicacionServices;
using Aplicacion.Interfaces.Infraestructura;
using Dominio.Modelos.Entidades;

namespace Aplicacion.Servicios
{
    public class AuthenticationService : IAuthService
    {
        private readonly IUsuarioRepository _repo;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IUsuarioRepository repo, ITokenService tokenService)
        {
            _repo = repo;
            _tokenService = tokenService;
        }

        public async Task<string> Login(string email, string password)
        {

            Usuario? usuario = await _repo.ObtenerPorEmail(email);

            if (usuario == null)
            {
                throw new InvalidLoginException("Email no encontrado. Verifica que todo este escrito correctamente");
            }


            if (!BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash))
                throw new InvalidLoginException("Contraseña invalida");


            return _tokenService.GenerateToken(email);
        }
    }
}

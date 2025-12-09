using Aplicacion.Interfaces.AplicacionServices;
using Aplicacion.Exceptions;
using Aplicacion.DTOs.UsuarioEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentación.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUsuarioService _usuarioService;

        public AuthController(IAuthService authService, IUsuarioService usuarioService)
        {
            _authService = authService;
            _usuarioService = usuarioService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            try
            {
                var token = await _authService.Login(dto.Email, dto.Password);
                return Ok(new { token });
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UsuarioRequestDTO dto)
        {
            try
            {
                _usuarioService.Guardar(dto); // Guardar es síncrono según tu UsuarioService

                // Login es asíncrono, necesita await
                var token = await _authService.Login(dto.Email, dto.Password);
                return Ok(new { token });
            }
            catch (EmailAlreadyInUseException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}

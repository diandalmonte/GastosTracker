using Aplicacion.Interfaces.AplicacionServices;
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
        public IActionResult Login(LoginDTO dto)
        {
            try
            {
                var token = _authService.Login(dto.Email, dto.Password);
                return Ok(new { token });
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpPost("register")]
        public IActionResult Register(UsuarioCreateDTO dto)
        {
            try
            {
                string password = dto.Password;
                _usuarioService.Guardar(dto);

                // Login automático después del registro
                var token = _authService.Login(dto.Email, password);
                return Ok(new { token });
            }
            catch (EmailAlreadyInUse ex)
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

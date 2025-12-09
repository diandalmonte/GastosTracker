using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Aplicacion.Interfaces.AplicacionServices;
using Aplicacion.DTOs.UsuarioEntity;
using Aplicacion.Exceptions;
using System.IdentityModel.Tokens.Jwt;

namespace Presentacion.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        private Guid GetLoggedUserId()
        {
            var idString = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrEmpty(idString))
            {
                throw new UnauthorizedAccessException("Token inválido.");
            }

            return Guid.Parse(idString);
        }

        [HttpGet("perfil")]
        public ActionResult<UsuarioResponseDTO> GetPerfil()
        {
            try
            {
                var userId = GetLoggedUserId();
                var usuario = _usuarioService.ObtenerPorId(userId);
                return Ok(usuario);
            }
            catch (ItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("perfil")]
        public IActionResult ActualizarPerfil([FromBody] UsuarioRequestDTO dto)
        {
            try
            {
                dto.Id = GetLoggedUserId();

                _usuarioService.Actualizar(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("perfil")]
        public IActionResult EliminarCuenta()
        {
            try
            {
                var userId = GetLoggedUserId();
                _usuarioService.Eliminar(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

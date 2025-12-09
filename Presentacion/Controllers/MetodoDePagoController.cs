using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Aplicacion.Interfaces.AplicacionServices;
using Aplicacion.DTOs.MetodoDePagoEntity;
using Aplicacion.Exceptions;
using System.IdentityModel.Tokens.Jwt;

namespace Presentacion.Controllers
{
    [ApiController]
    [Route("api/metodos-pago")]
    [Authorize]
    public class MetodoDePagoController : ControllerBase
    {
        private readonly IMetodoDePagoService _service;

        public MetodoDePagoController(IMetodoDePagoService service)
        {
            _service = service;
        }

        private Guid GetLoggedUserId()
        {
            var idString = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(idString))
                throw new UnauthorizedAccessException("Token inválido.");
            return Guid.Parse(idString);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MetodoDePagoReadDTO>>> GetMetodos()
        {
            try
            {
                var userId = GetLoggedUserId();
                var metodos = await _service.Obtener(userId);
                return Ok(metodos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MetodoDePagoReadDTO>> GetMetodo(Guid id)
        {
            try
            {
                var userId = GetLoggedUserId();
                var metodo = await _service.ObtenerPorId(id, userId);
                return Ok(metodo);
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

        [HttpPost]
        public IActionResult PostMetodo([FromBody] MetodoDePagoCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                dto.UsuarioId = GetLoggedUserId();

                _service.Guardar(dto);
                return StatusCode(201, "Método de pago creado");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutMetodo(Guid id, [FromBody] MetodoDePagoCreateDTO dto)
        {
            try
            {
                if (dto.Id != Guid.Empty && id != dto.Id)
                    return BadRequest("ID URL no coincide con ID Cuerpo");

                dto.UsuarioId = GetLoggedUserId();
                dto.Id = id;

                _service.Actualizar(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMetodo(Guid id)
        {
            try
            {
                var userId = GetLoggedUserId();
                _service.Eliminar(id, userId);
                return NoContent();
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
    }
}

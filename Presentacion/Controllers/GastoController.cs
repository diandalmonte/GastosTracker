using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Aplicacion.Interfaces.AplicacionServices;
using Aplicacion.DTOs.GastoEntity;
using Aplicacion.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using Dominio.Exceptions;

namespace Presentacion.Controllers
{
    [ApiController]
    [Route("api/gastos")]
    [Authorize]
    public class GastoController : ControllerBase
    {
        private readonly IGastoService _gastoService;

        public GastoController(IGastoService gastoService)
        {
            _gastoService = gastoService;
        }

        private Guid GetLoggedUserId()
        {
            var idString = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(idString))
                throw new UnauthorizedAccessException("Token inválido: No contiene ID.");
            return Guid.Parse(idString);
        }

        [HttpGet("usuario/")]
        public ActionResult<List<GastoVistaPrevia>> GetGastos()
        {
            try
            {
                var idUsuario = GetLoggedUserId();

                var gastos = _gastoService.ObtenerVistasPrevias(idUsuario);
                return Ok(gastos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GastoReadDTO>> GetGasto(Guid id)
        {
            try
            {
                var userId = GetLoggedUserId();
                var gasto = await _gastoService.ObtenerPorId(id, userId);
                return Ok(gasto);
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
        public async Task<ActionResult> PostGasto([FromBody] GastoCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                dto.UsuarioId = GetLoggedUserId();

                // 'false' indica que se crea manualmente (valida presupuesto)
                var alertas = await _gastoService.Guardar(dto, isImported: false);

                return StatusCode(201, new { Message = "Gasto registrado.", Alertas = alertas });
            }
            catch (NegativeValueException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ModelConstructionException ex)
            {
                return BadRequest(ex.Message);
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

        [HttpPut("{id}")]
        public IActionResult PutGasto(Guid id, [FromBody] GastoCreateDTO dto)
        {
            try
            {
                if (dto.Id != null && dto.Id != Guid.Empty && id != dto.Id)
                {
                    return BadRequest("El id de la url no coincide con el cuerpo.");
                }
                    

                dto.UsuarioId = GetLoggedUserId();
                dto.Id = id; 

                _gastoService.Actualizar(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGasto(Guid id)
        {
            try
            {
                var userId = GetLoggedUserId();
                _gastoService.Eliminar(id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("buscar")]
        public async Task<ActionResult<IEnumerable<GastoVistaPrevia>>> BuscarGastos([FromBody] GastoFilter filtro)
        {
            try
            {
                var userId = GetLoggedUserId();
                var gastos = await _gastoService.ObtenerPorFiltro(filtro, userId);
                return Ok(gastos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

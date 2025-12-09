using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Aplicacion.Interfaces.AplicacionServices;
using Aplicacion.Servicios; // Necesario si casteamos
using System.IdentityModel.Tokens.Jwt;

namespace Presentacion.Controllers
{
    [ApiController]
    [Route("api/presupuestos")]
    [Authorize]
    public class PresupuestoController : ControllerBase
    {
        private readonly IPresupuestoService _service;

        public PresupuestoController(IPresupuestoService service)
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

        [HttpGet("general")]
        public async Task<ActionResult<decimal>> GetPresupuestoGeneral()
        {
            try
            {
                var userId = GetLoggedUserId();

                // NOTA: Agregar 'ObtenerPresupuestoGeneral' a IPresupuestoService para evitar este cast
                if (_service is PresupuestoService serviceConcreto)
                {
                    var presupuesto = await serviceConcreto.ObtenerPresupuestoGeneral(userId);
                    return Ok(presupuesto);
                }
                return StatusCode(501, "Método no expuesto en la interfaz del servicio.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("porcentaje-general")]
        public async Task<ActionResult<int>> GetPorcentajeGeneral()
        {
            try
            {
                var userId = GetLoggedUserId();
                if (_service is PresupuestoService serviceConcreto)
                {
                    var porcentaje = await serviceConcreto.ObtenerPorcentajePresupuestoGeneral(userId);
                    return Ok(porcentaje);
                }
                return StatusCode(501, "Método no expuesto en la interfaz del servicio.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("diferencia-general")]
        public async Task<ActionResult<decimal>> GetDiferenciaGeneral()
        {
            try
            {
                var userId = GetLoggedUserId();
                var diferencia = await _service.ObtenerDiferenciaGeneral(userId);
                return Ok(diferencia);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("excedidos")]
        public async Task<ActionResult> GetCategoriasExcedidas()
        {
            try
            {
                var userId = GetLoggedUserId();
                var excedidas = await _service.ObtenerCategoriasExcedidas(userId);
                return Ok(excedidas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("resumen-categorias")]
        public async Task<ActionResult> GetResumenCategorias()
        {
            try
            {
                var userId = GetLoggedUserId();
                var procesadas = await _service.ObtenerCategoriasProcesadas(userId);
                return Ok(procesadas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

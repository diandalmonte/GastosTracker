using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Aplicacion.Interfaces.AplicacionServices;
using Aplicacion.DTOs.ReporteEntity;
using System.IdentityModel.Tokens.Jwt;

namespace Presentacion.Controllers
{
    [ApiController]
    [Route("api/reportes")]
    [Authorize]
    public class ReporteController : ControllerBase
    {
        private readonly IReporteService _service;

        public ReporteController(IReporteService service)
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

        [HttpGet("mensual")]
        public async Task<ActionResult<ReporteMensualDTO>> GetReporteMensual()
        {
            try
            {
                var userId = GetLoggedUserId();
                var reporte = await _service.GenerarDatosReporte(userId);
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("exportar")]
        public async Task<IActionResult> ExportarReporte([FromQuery] string formato)
        {
            try
            {
                var userId = GetLoggedUserId();
                byte[] archivo = await _service.ExportarReporte(userId, formato);

                string mimeType = "application/json";
                string extension = "json";

                if (formato.Equals("Excel", StringComparison.OrdinalIgnoreCase))
                {
                    mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    extension = "xlsx";
                }
                else if (formato.Equals("Txt", StringComparison.OrdinalIgnoreCase))
                {
                    mimeType = "text/plain";
                    extension = "txt";
                }

                return File(archivo, mimeType, $"Reporte_{DateTime.Now:yyyyMMdd}.{extension}");
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

        [HttpPost("importar")]
        public async Task<IActionResult> ImportarGastos(IFormFile archivo)
        {
            try
            {
                if (archivo == null || archivo.Length == 0)
                    return BadRequest("No se ha subido ningún archivo.");

                var userId = GetLoggedUserId();

                using (var stream = archivo.OpenReadStream())
                {
                    await _service.ImportarGastos(stream, userId);
                }

                return Ok("Importación completada.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al importar: {ex.Message}");
            }
        }
    }
}

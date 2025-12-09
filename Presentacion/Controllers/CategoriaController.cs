using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Aplicacion.Interfaces.AplicacionServices;
using Aplicacion.DTOs.CategoriaEntity;
using Aplicacion.Exceptions;
using System.IdentityModel.Tokens.Jwt;

namespace Presentacion.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    [Authorize]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        private Guid GetLoggedUserId()
        {
            var idString = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(idString))
                throw new UnauthorizedAccessException("Token inválido: No contiene ID.");
            return Guid.Parse(idString);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaReadDTO>>> GetCategorias()
        {
            try
            {
                var userId = GetLoggedUserId(); // ID directo del token
                var categorias = await _categoriaService.Obtener(userId);
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaReadDTO>> GetCategoria(Guid id)
        {
            try
            {
                var userId = GetLoggedUserId();
                var categoria = await _categoriaService.ObtenerPorId(id, userId);
                return Ok(categoria);
            }
            catch (ItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult PostCategoria([FromBody] CategoriaCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                dto.UsuarioId = GetLoggedUserId();

                _categoriaService.Guardar(dto);
                return StatusCode(201, "Categoría creada exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutCategoria(Guid id, [FromBody] CategoriaCreateDTO dto)
        {
            try
            {
                if (dto.Id != null && dto.Id != Guid.Empty && id != dto.Id)
                    return BadRequest("El ID de la URL no coincide con el cuerpo.");

                dto.UsuarioId = GetLoggedUserId();
                dto.Id = id;

                _categoriaService.Actualizar(dto);
                return NoContent();
            }
            catch (ItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategoria(Guid id)
        {
            try
            {
                var userId = GetLoggedUserId();
                _categoriaService.Eliminar(id, userId);
                return NoContent();
            }
            catch (ItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<CategoriaReadDTO>>> BuscarCategorias([FromQuery] string filtro)
        {
            try
            {
                var userId = GetLoggedUserId();
                var categorias = await _categoriaService.ObtenerPorFiltro(filtro, userId);
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}

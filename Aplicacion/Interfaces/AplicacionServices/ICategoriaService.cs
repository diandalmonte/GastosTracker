using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.CategoriaEntity;

namespace Aplicacion.Interfaces.AplicacionServices
{
    public interface ICategoriaService
    {
        public void Guardar(CategoriaCreateDTO dto);
        public Task<IEnumerable<CategoriaReadDTO>> Obtener(Guid idUsuario);
        public Task<CategoriaReadDTO> ObtenerPorId(Guid id, Guid idUsuario);
        public void Actualizar(CategoriaCreateDTO dto);
        public void Eliminar(Guid id, Guid idUsuario);
        public Task<IEnumerable<CategoriaReadDTO>> ObtenerPorFiltro(string filtro, Guid idUsuario);
    }
}

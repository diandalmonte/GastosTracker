using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.GastoEntity;
using Dominio.Modelos.Entidades;

namespace Aplicacion.Interfaces.AplicacionServices
{
    public interface IGastoService
    {
        public Task<List<string>> Guardar(GastoCreateDTO dto, bool isImported);
        public Task<List<Gasto>> Obtener(Guid idUsuario);
        public Task<PagedResult> ObtenerVistasPrevias(Guid idUsuario); //Obtener paginadas? (Por fecha?)
        public Task<GastoReadDTO> ObtenerPorId(Guid id, Guid idUsuario);
        public void Actualizar(GastoCreateDTO dto);
        public void Eliminar(Guid id, Guid idUsuario);
        public Task<IEnumerable<GastoVistaPrevia>> ObtenerPorFiltro(GastoFilter filtro, Guid idUsuario); //Obtener paginadas? (Por fecha?)
    }
}

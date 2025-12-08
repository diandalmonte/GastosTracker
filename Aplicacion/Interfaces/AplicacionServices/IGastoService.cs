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
        public void Guardar(GastoCreateDTO dto);
        public Task<PagedResult> ObtenerVistasPrevias(); //Obtener paginadas? (Por fecha?)
        public GastoReadDTO ObtenerPorId(Guid id);
        public void Actualizar(GastoCreateDTO dto);
        public void Eliminar(Guid id);
        public IEnumerable<GastoVistaPrevia> ObtenerPorFiltro(GastoFilter filtro); //Obtener paginadas? (Por fecha?)
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.GastoEntity;
using Aplicacion.Interfaces.AplicacionServices;
using Aplicacion.Exceptions;
using AutoMapper;

namespace Aplicacion.Servicios
{
    public class GastoService : IGastoService
    {
        public void Guardar(GastoCreateDTO dto)
        {
            if (dto.IsFechaActual)
            {
                dto.Fecha = DateOnly.FromDateTime(DateTime.UtcNow);
            } 
            else if (!dto.IsFechaActual && !dto.Fecha.HasValue)
            {
                throw new ModelConstructionException("Error en la asignacion de fecha para Gasto");
            }

        }

        public IEnumerable<GastoVistaPrevia> ObtenerPorFiltro(GastoFilter filtro)
        {
            throw new NotImplementedException();
        }

        public GastoReadDTO ObtenerPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult> ObtenerVistasPrevias()
        {
            throw new NotImplementedException();
        }

        public void Actualizar(GastoCreateDTO dto)
        {
            throw new NotImplementedException();
        }

        public void Eliminar(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

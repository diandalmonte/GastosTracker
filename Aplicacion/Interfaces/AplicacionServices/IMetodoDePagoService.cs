using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs;
using Dominio.Modelos.Entidades;
using Aplicacion.DTOs.MetodoDePagoEntity;

namespace Aplicacion.Interfaces.AplicacionServices
{
    public interface IMetodoDePagoService
    {
        public void Guardar(MetodoDePagoCreateDTO dto);
        public Task<IEnumerable<MetodoDePagoReadDTO>> Obtener(Guid idUsuario);
        public Task<MetodoDePagoReadDTO> ObtenerPorId(Guid id, Guid idUsuario);
        public void Actualizar(MetodoDePagoCreateDTO dto);
        public void Eliminar(Guid id, Guid idUsuario);
    }
}

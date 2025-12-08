using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs;
using Dominio.Modelos.Entidades;
using Aplicacion.DTOs.MetodoDePago;

namespace Aplicacion.Interfaces.AplicacionServices
{
    public interface IMetodoDePagoService
    {
        public void Guardar(MetodoDePagoCreateDTO dto);
        public IEnumerable<MetodoDePagoReadDTO> Obtener();
        public MetodoDePagoReadDTO ObtenerPorId(Guid id);
        public void Actualizar(MetodoDePagoCreateDTO dto);
        public void Eliminar(Guid id);
    }
}

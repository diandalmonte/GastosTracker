using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Modelos.Entidades;

namespace Aplicacion.DTOs.GastoEntity
{
    public class GastoFilter
    {
        public string? ContieneString { get; set; }
        public Guid? CategoriaId { get; set; }
        public Guid? MetodoDePagoId { get; set; }
        public DateOnly? fechaInicio { get; set; }
        public DateOnly? fechaFin { get; set; }
    }
}

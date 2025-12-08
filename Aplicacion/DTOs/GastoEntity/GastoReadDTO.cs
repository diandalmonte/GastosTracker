using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs.GastoEntity
{
    public class GastoReadDTO
    {
        public Guid Id { get; set; }
        public string Encabezado { get; set; }
        public string NombreCategoria { get; set; }
        public decimal Monto { get; set; }
        public string MetodoDePago { get; set; }
        public string? Descripcion { get; set; }
        public DateOnly Fecha { get; set; }
    }
}

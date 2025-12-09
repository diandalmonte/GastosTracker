using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs.ReporteEntity
{
    public class CategoriaReporteDTO
    {
        public string NombreCategoria { get; set; }
        public decimal TotalGastado { get; set; }
        public decimal PorcentajeDelTotal { get; set; }
    }
}

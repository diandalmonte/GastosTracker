using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs.ReporteEntity
{
    public class ComparativaReporteDTO
    {
        public decimal TotalMesActual { get; set; }
        public decimal TotalMesAnterior { get; set; }
        public decimal VariacionMonto { get; set; } // Diferencia absoluta
        public decimal VariacionPorcentual { get; set; } // % de cambio
        public bool EsAhorro { get; set; } // True si gastaste menos que el mes anterior
    }
}

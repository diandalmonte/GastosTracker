using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs.ReporteEntity
{
    public class ReporteMensualDTO
    {
        public string Periodo { get; set; } 
        public decimal TotalGastado { get; set; }
        public decimal PresupuestoGeneral { get; set; }
        public decimal DiferenciaPresupuesto { get; set; }

        public ComparativaReporteDTO Comparativa { get; set; }
        public List<CategoriaReporteDTO> GastosPorCategoria { get; set; } = new();
        public List<CategoriaReporteDTO> TopCategorias { get; set; } = new(); //Toma las 3 en las que mas se gastó
    }
}

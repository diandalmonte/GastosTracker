using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.ReporteEntity;
using Aplicacion.Interfaces.Infraestructura;

namespace Infraestructura.Exportacion
{
    public class ReporteTxtExporter : IReporteExporter
    {
        public string Formato => "Txt";

        public byte[] Exportar(ReporteMensualDTO datos)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"REPORTE DE GASTOS - {datos.Periodo}");
            sb.AppendLine("========================================");
            sb.AppendLine($"Total Gastado:     {datos.TotalGastado:C2}");
            sb.AppendLine($"Presupuesto Total: {datos.PresupuestoGeneral:C2}");
            sb.AppendLine($"Estado:            {(datos.DiferenciaPresupuesto >= 0 ? "Bajo Presupuesto" : "Excedido")}");
            sb.AppendLine();
            sb.AppendLine("--- COMPARATIVA MES ANTERIOR ---");
            sb.AppendLine($"Mes Anterior:      {datos.Comparativa.TotalMesAnterior:C2}");
            sb.AppendLine($"Variacion:         {datos.Comparativa.VariacionPorcentual}%");
            sb.AppendLine();
            sb.AppendLine("--- TOP CATEGORIAS ---");
            foreach (var item in datos.TopCategorias)
            {
                sb.AppendLine($"- {item.NombreCategoria}: {item.TotalGastado:C2} ({item.PorcentajeDelTotal}%)");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}

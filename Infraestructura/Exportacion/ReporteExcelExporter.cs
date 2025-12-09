using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.ReporteEntity;
using Aplicacion.Interfaces.Infraestructura;
using OfficeOpenXml;

namespace Infraestructura.Exportacion
{
    public class ReporteExcelExporter : IReporteExporter
    {
        public string Formato => "Excel";

        public byte[] Exportar(ReporteMensualDTO datos)
        {

            using (var package = new ExcelPackage())
            {
                var sheet = package.Workbook.Worksheets.Add("Reporte Mensual");

                // Encabezados
                sheet.Cells["A1"].Value = $"Reporte de Gastos - {datos.Periodo}";
                sheet.Cells["A1:D1"].Merge = true;
                sheet.Cells["A1"].Style.Font.Bold = true;
                sheet.Cells["A1"].Style.Font.Size = 14;

                sheet.Cells["A3"].Value = "Total Gastado";
                sheet.Cells["B3"].Value = datos.TotalGastado;

                sheet.Cells["A4"].Value = "Presupuesto";
                sheet.Cells["B4"].Value = datos.PresupuestoGeneral;

                sheet.Cells["A5"].Value = "Variación vs Mes Anterior";
                sheet.Cells["B5"].Value = $"{datos.Comparativa.VariacionPorcentual}%";

                // Tabla de categorias
                sheet.Cells["A7"].Value = "Categoría";
                sheet.Cells["B7"].Value = "Monto";
                sheet.Cells["C7"].Value = "% del Total";
                sheet.Cells["A7:C7"].Style.Font.Bold = true;

                int row = 8;
                foreach (var cat in datos.GastosPorCategoria)
                {
                    sheet.Cells[row, 1].Value = cat.NombreCategoria;
                    sheet.Cells[row, 2].Value = cat.TotalGastado;
                    sheet.Cells[row, 3].Value = $"{cat.PorcentajeDelTotal}%";
                    row++;
                }

                sheet.Cells.AutoFitColumns();
                return package.GetAsByteArray();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Aplicacion.DTOs.ReporteEntity;
using Aplicacion.Interfaces.Infraestructura;

namespace Infraestructura.Exportacion
{
    public class ReporteJsonExporter : IReporteExporter
    {
        public string Formato => "Json";

        public byte[] Exportar(ReporteMensualDTO datos)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(datos, options);
            return Encoding.UTF8.GetBytes(jsonString);
        }
    }
}

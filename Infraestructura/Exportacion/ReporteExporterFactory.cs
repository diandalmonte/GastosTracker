using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Interfaces.Infraestructura;

namespace Infraestructura.Exportacion
{
    public class ReporteExporterFactory
    {
        private readonly IEnumerable<IReporteExporter> _exporters;

        public ReporteExporterFactory(IEnumerable<IReporteExporter> exporters)
        {
            _exporters = exporters;
        }

        public IReporteExporter ObtenerExporter(string formato)
        {
            var exporter = _exporters.FirstOrDefault(e => e.Formato.Equals(formato, StringComparison.OrdinalIgnoreCase));

            if (exporter == null)
                throw new ArgumentException("Formato de exportación no soportado");

            return exporter;
        }
    }
}

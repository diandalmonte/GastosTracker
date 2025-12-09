using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.ReporteEntity;

namespace Aplicacion.Interfaces.Infraestructura
{
    public interface IReporteExporter
    {
        byte[] Exportar(ReporteMensualDTO datos);
        string Formato { get; } // Tipo de archivo
    }
}

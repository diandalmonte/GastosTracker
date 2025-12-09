using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.ReporteEntity;

namespace Aplicacion.Interfaces.AplicacionServices
{
    public interface IReporteService
    {
        Task<ReporteMensualDTO> GenerarDatosReporte(Guid usuarioId); // CU10
        Task<byte[]> ExportarReporte(Guid usuarioId, string formato); // CU11
        Task ImportarGastos(Stream archivo, Guid usuarioId); // CU07
    }
}

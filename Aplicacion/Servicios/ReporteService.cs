using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.GastoEntity;
using Aplicacion.DTOs.ReporteEntity;
using Aplicacion.Interfaces.AplicacionServices;
using Aplicacion.Interfaces.Infraestructura;
using Aplicacion.Utilidades;
using Dominio.Modelos.Entidades;

namespace Aplicacion.Servicios
{
    public class ReporteService : IReporteService
    {
        private readonly IFiltrableRepository<Gasto, GastoFilter> _repoGastos;
        private readonly IUsuarioRepository _repoUsuario;
        private readonly ReporteExporterFactory _exporterFactory;
        private readonly IGastoImport _gastoImporter;
        private readonly IGastoService _gastoService; // Para reutilizar lógica de guardado

        public ReporteService(
            IFiltrableRepository<Gasto, GastoFilter> repoGastos,
            IUsuarioRepository repoUsuario,
            ReporteExporterFactory exporterFactory,
            IGastoImport gastoImporter,
            IGastoService gastoService)
        {
            _repoGastos = repoGastos;
            _repoUsuario = repoUsuario;
            _exporterFactory = exporterFactory;
            _gastoImporter = gastoImporter;
            _gastoService = gastoService;
        }

        // CU10: Generar Datos
        public async Task<ReporteMensualDTO> GenerarDatosReporte(Guid usuarioId)
        {
            // 1. Obtener rangos de fecha
            var (inicioMes, finMes) = DateExtensions.ObtenerRangoMesActual();
            var (inicioMesAnt, finMesAnt) = DateTime.UtcNow.ObtenerRangoMesPasado();

            // 2. Obtener gastos del mes actual y anterior
            var gastosActuales = await _repoGastos.ObtenerPorFiltro(new GastoFilter
            { FechaInicio = inicioMes, FechaFin = finMes }, usuarioId);

            var gastosAnteriores = await _repoGastos.ObtenerPorFiltro(new GastoFilter
            { FechaInicio = inicioMesAnt, FechaFin = finMesAnt }, usuarioId);

            var usuario = await _repoUsuario.ObtenerPorId(usuarioId);
            decimal presupuesto = usuario?.Presupuesto ?? 0;

            // 3. Cálculos Generales
            decimal totalActual = gastosActuales.Sum(g => g.Monto);
            decimal totalAnterior = gastosAnteriores.Sum(g => g.Monto);

            // 4. Agrupación por Categorías (LINQ)
            var porCategoria = gastosActuales
                .GroupBy(g => g.Categoria?.Nombre ?? "Sin Categoría")
                .Select(grupo => new CategoriaReporteDTO
                {
                    NombreCategoria = grupo.Key,
                    TotalGastado = grupo.Sum(g => g.Monto),
                    PorcentajeDelTotal = totalActual > 0
                        ? Math.Round((grupo.Sum(g => g.Monto) / totalActual) * 100, 2)
                        : 0
                })
                .OrderByDescending(c => c.TotalGastado)
                .ToList();

            // 5. Comparativa
            var comparativa = new ComparativaReporteDTO
            {
                TotalMesActual = totalActual,
                TotalMesAnterior = totalAnterior,
                VariacionMonto = totalActual - totalAnterior,
                EsAhorro = totalActual < totalAnterior
            };

            if (totalAnterior > 0)
                comparativa.VariacionPorcentual = Math.Round(((totalActual - totalAnterior) / totalAnterior) * 100, 2);
            else
                comparativa.VariacionPorcentual = 100; // Si antes era 0 y ahora gastaste, aumentó 100% (o infinito)

            // 6. Armar DTO Final
            return new ReporteMensualDTO
            {
                Periodo = $"{DateTime.UtcNow:MMMM yyyy}",
                TotalGastado = totalActual,
                PresupuestoGeneral = presupuesto,
                DiferenciaPresupuesto = presupuesto - totalActual,
                Comparativa = comparativa,
                GastosPorCategoria = porCategoria,
                Top3Categorias = porCategoria.Take(3).ToList()
            };
        }
}

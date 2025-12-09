using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Utilidades
{
    public static class DateExtensions
    {
        public static (DateOnly Inicio, DateOnly Fin) ObtenerRangoMesActual()
        {
            var fecha = DateOnly.FromDateTime(DateTime.UtcNow);
            var inicio = new DateOnly(fecha.Year, fecha.Month, 1);
            var fin = inicio.AddMonths(1).AddDays(-1);

            return (inicio, fin);
        }

        public static (DateOnly Inicio, DateOnly Fin) ObtenerRangoMesPasado(this DateTime fechaBase)
        {
            var fechaMesPasado = DateOnly.FromDateTime(DateTime.UtcNow).AddMonths(-1);
            var inicio = new DateOnly(fechaMesPasado.Year, fechaMesPasado.Month, 1);
            var fin = inicio.AddMonths(1).AddDays(-1);

            return (inicio, fin);
        }
    }
}

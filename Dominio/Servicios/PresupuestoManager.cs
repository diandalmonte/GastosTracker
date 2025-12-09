using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Dominio.Modelos.Entidades;
namespace Dominio.Servicios
{
    public class PresupuestoManager : IPresupuestoManager
    {
        private const decimal Limite = 1.0m;
        private const decimal CantidadAlertaCritica = 0.8m;
        private const decimal CantidadAlertaMitad = 0.5m;

        public List<string> ValidarPresupuesto(Categoria categoria, decimal montoGastoNuevo,
                                               decimal presupuestoCategoria, decimal gastosEnCategoria,
                                               decimal presupuestoGeneral, decimal gastosGenerales)
        {
            List<string> alertas = [];

            CheckPresupuesto(
                alertas,
                montoGastoNuevo,
                gastosEnCategoria,
                presupuestoCategoria,
                $"la categoría '{categoria.Nombre}'"
            );

            CheckPresupuesto(
                alertas,
                montoGastoNuevo,
                gastosGenerales,
                presupuestoGeneral,
                "el presupuesto global"
            );

            return alertas;
        }

        //Con este metodo me aseguro de no tener que repetir la misma logica dos veces, solo llamar al metodo dos veces
        private void CheckPresupuesto(List<string> alertas, decimal nuevoGasto, decimal gastadoActual, decimal limite, string nombreEntidad)
        {
            if (limite == 0)
            {
                alertas.Add($"Error: El presupuesto para {nombreEntidad} es 0, no se pueden calcular alertas.");
                return;
            }

            decimal gastosTotales = nuevoGasto + gastadoActual;
            decimal porcentaje = gastosTotales / limite;

            if (porcentaje >= Limite)
            {
                alertas.Add($"Límite alcanzado en {nombreEntidad}!");
            }
            else if (porcentaje >= CantidadAlertaCritica)
            {
 
                alertas.Add($"Aviso: Un {(int)(CantidadAlertaCritica * 100)}% de {nombreEntidad} ha sido consumido.");
            }
            else if (porcentaje >= CantidadAlertaMitad)
            {
                alertas.Add($"Aviso: Un {(int)(CantidadAlertaMitad * 100)}% de {nombreEntidad} ha sido consumido.");
            }
        }
    }
}
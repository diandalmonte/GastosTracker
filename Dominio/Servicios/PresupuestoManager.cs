using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Dominio.Modelos.Entidades;

namespace Dominio.Servicios
/*
    public decimal TotalGastado { get; set; }
    public decimal PorcentajeConsumido { get; set; }
    public List<string>? Alertas { get; set; }
    public bool EstaExcedido { get; set; }
*/
{
    public class PresupuestoManager : IPresupuestoManager
    {
        public List<string> ValidarPresupuesto(decimal montoGastoNuevo, Presupuesto presupuestoCategoria, decimal gastosEnCategoria,
                                                     Presupuesto presupuestoGeneral, decimal gastosGenerales)
        {
            List<string> alertas = [];
            if (presupuestoCategoria != null)
            {
                decimal gastosTotales = montoGastoNuevo + gastosEnCategoria;
                decimal porcentajeCat = (gastosTotales / presupuestoCategoria.MontoLimite);

                if (porcentajeCat >= 1.0m)
                {
                    alertas.Add($"Limite de Presupuesto Mensual alcanzado en categoria: {presupuestoCategoria.Categoria?.Nombre}!");
                }
                else if(porcentajeCat >= 0.8m)
                {
                    alertas.Add($"Aviso: Un 80% del presupuesto mensual para la categoria '{presupuestoCategoria.Categoria?.Nombre}' ha sido consumido!");
                }
                else if (porcentajeCat >= 0.5m)
                {
                    alertas.Add($"Aviso: Un 50% del presupuesto mensual para la categoria '{presupuestoCategoria.Categoria?.Nombre}' ha sido consumido!");
                }
            }


            if (presupuestoCategoria != null)
            {
                decimal gastosTotales = montoGastoNuevo + gastosGenerales;
                decimal porcentajeGeneral = (gastosTotales / presupuestoGeneral.MontoLimite);

                if (porcentajeGeneral >= 1.0m)
                {
                    alertas.Add($"Limite de Presupuesto Global alcanzado!");
                }
                else if (porcentajeGeneral >= 0.8m)
                {
                    alertas.Add($"Aviso: Un 80% del presupuesto global ha sido consumido!");
                }
                else if (porcentajeGeneral >= 0.5m)
                {
                    alertas.Add($"Aviso: Un 50% del presupuesto global ha sido consumido!");
                }
            }

            return alertas;
        }
    }
}

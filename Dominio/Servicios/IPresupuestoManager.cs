using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Modelos.Entidades;

namespace Dominio.Servicios
{
    public interface IPresupuestoManager
    {
        public List<string> ValidarPresupuesto(
            decimal montoGastoNuevo, 
            Presupuesto presupuestoCategoria, 
            decimal gastosEnCategoria,
            Presupuesto presupuestoGeneral,
            decimal gastosGenerales);
    }
}

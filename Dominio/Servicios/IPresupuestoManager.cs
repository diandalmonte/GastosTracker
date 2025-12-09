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
            Categoria categoria,
            decimal montoGastoNuevo, 
            decimal presupuestoCategoria, 
            decimal gastosEnCategoria,
            decimal presupuestoGeneral,
            decimal gastosGenerales);
    }
}

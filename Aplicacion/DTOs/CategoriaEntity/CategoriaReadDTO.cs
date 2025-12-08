using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs.CategoriaEntity
{
    public class CategoriaReadDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public decimal? MontoPresupuesto { get; set; }
        public int? PorcentajePresupuesto { get; set; }
        public bool IsExcedido { get; set; }
    }
}

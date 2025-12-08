using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Modelos.Entidades;

namespace Aplicacion.DTOs.GastoEntity
{
    public class PagedResult
    {
        public List<Gasto> Items { get; set; }
        public int TotalGastos { get; set; }
        public int PaginaActual { get; set; }
        public int PaginaSize { get; set; }
        public int TotalPaginas { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs.MetodoDePago
{
    public class MetodoDePagoReadDTO
    {
        public Guid Id { get; set; }
        public string TipoPago { get; set; }
        public string Nombre { get; set; }
    }
}

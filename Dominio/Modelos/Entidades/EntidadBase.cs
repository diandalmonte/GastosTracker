using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos.Entidades
{
    public abstract class EntidadBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}

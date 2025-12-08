using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs.Gasto
{
    public class GastoCreateDTO
    {
        public string Encabezado { get; set; }
        public decimal Monto { get; private set; } //CAMBIAR: Validar que esto sea solo positivo
        public Guid CategoriaId { get; set; }
        public Guid MetodoDePagoId { get; set; }
        public Guid UsuarioId { get; set; }
        public string? Descripcion { get; set; }
        public DateOnly Fecha { get; set; }
    }
}

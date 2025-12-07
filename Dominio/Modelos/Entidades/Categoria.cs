using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Modelos.Entidades
{
    public class Categoria : EntidadBase
    {
        public string Nombre { get; set; }
        public Guid PresupuestoId { get; set; }

        [ForeignKey("PresupuestoId")]
        public Presupuesto? Presupuesto { get; set; }
        public Guid UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }//PORQUE porque null

        public Categoria(string nombre, decimal montoPresupuesto, Guid usuarioId)
        {

        }

        public bool IsPresupuestoExcedido(decimal montoGastos)
        {
            return (Presupuesto?.MontoLimite < montoGastos);
        }

        public decimal? GetDiferenciaPresupuesto(decimal montoGastos)
        {
            return (Presupuesto?.MontoLimite - montoGastos);
        }
    }
}

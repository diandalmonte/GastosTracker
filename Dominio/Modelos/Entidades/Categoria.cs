using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Modelos.Entidades
{
    public class Categoria : EntidadBase, IEntidadDeUsuario
    {
        public string Nombre { get; set; }
        public decimal Presupuesto { get; set; }
        public Guid UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }//PORQUE porque null

        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

        public Categoria(string nombre, decimal presupuesto, Guid usuarioId)
        {
            Nombre = nombre;
            Presupuesto = presupuesto;
            UsuarioId = usuarioId;
        }

        public bool IsPresupuestoExcedido(decimal montoGastos)
        {
            return (Presupuesto < montoGastos);
        }

        public decimal? GetDiferenciaPresupuesto(decimal montoGastos)
        {
            return (Presupuesto - montoGastos);
        }
    }
}

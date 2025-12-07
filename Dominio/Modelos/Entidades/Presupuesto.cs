using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Exceptions;
using Dominio.Modelos.Enums;

namespace Dominio.Modelos.Entidades
{
    public class Presupuesto : EntidadBase
    {
        public Guid UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public Guid? CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public decimal MontoLimite { get; set; }

        public Presupuesto(Guid usuarioId, Guid? categoriaId, decimal montoLimite)
        {
            SetMontoLimite(montoLimite);
            UsuarioId = usuarioId;
            CategoriaId = categoriaId;
        }

        // Validación de dominio simple
        private void SetMontoLimite(decimal monto)
        {
            if (monto <= 0)
            {

                throw new NegativeValueException("El monto de un gasto debe ser positivo.");
            }
            else
            {
                this.MontoLimite = monto;
            }
        }
    }
}

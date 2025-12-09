using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Modelos.Enums;

namespace Dominio.Modelos.Entidades
{
    public class MetodoDePago : EntidadBase, IEntidadDeUsuario
    {
        public TipoPago TipoDePago { get; set; }
        public string Nombre { get; set; }
        public Guid UsuarioId { get; set; } // Importante para consultas rápidas por usuario

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Modelos.Entidades;

namespace Aplicacion.DTOs.CategoriaEntity
{
    public class CategoriaCreateDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public decimal MontoPresupuesto { get; set; }
        public Guid UsuarioId { get; set; }
    }
}

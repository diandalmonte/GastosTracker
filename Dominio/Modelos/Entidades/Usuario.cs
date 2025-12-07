using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos.Entidades
{
    public class Usuario : EntidadBase
    {
        public string Nombre { get; set; }
        public ICollection<Categoria> Categorias { get; set; } = [];
        public ICollection<MetodoDePago> MetodosDePago { get; set; } = [];
        public ICollection<Gasto> Gastos { get; set; } = [];
        public string PasswordHash { get; set; }
    }
}

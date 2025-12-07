using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Persistencia.Contexto
{
    public class GastosTrackerContext : DbContext
    {
        public GastosTrackerContext(DbContextOptions<GastosTrackerContext> db) : base(db)
        {

        }

        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<MetodoDePago> MetodosDePago { get; set; }
        public DbSet<Presupuesto> Presupuestos { get; set; }
        
    }
}

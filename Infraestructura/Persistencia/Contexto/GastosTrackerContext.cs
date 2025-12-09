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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gasto>()
                .HasOne(g => g.Usuario)
                .WithMany(u => u.Gastos)
                .HasForeignKey(g => g.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Gasto>()
                .HasOne(g => g.Categoria)
                .WithMany(c => c.Gastos)
                .HasForeignKey(g => g.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Gasto>()
                .HasOne(g => g.MetodoDePago)
                .WithMany(m => m.Gastos)
                .HasForeignKey(g => g.MetodoDePagoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

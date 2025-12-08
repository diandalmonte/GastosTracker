using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs;
using Aplicacion.Interfaces;
using Dominio.Modelos.Entidades;
using Infraestructura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Persistencia.Repositorios
{
    public class CategoriaRepository : IFiltrableRepository<Categoria, CategoriaFilter>
    {
        private readonly GastosTrackerContext _context;
        private readonly DbSet<Categoria> dbSet;

        public CategoriaRepository(GastosTrackerContext context)
        {
            _context = context;
            dbSet = _context.Set<Categoria>();
        }

        public void Guardar(Categoria categoria)
        {
            dbSet.Add(categoria);
            _context.SaveChanges();
        }

        public Categoria ObtenerPorId(Guid id)
        {
            return dbSet.Find(id);
        }
        public IEnumerable<Categoria> Obtener()
        {
            return dbSet.ToList();
        }

        public void Actualizar(Categoria categoria)
        {
            dbSet.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Eliminar(Guid id)
        {
            Categoria categoria = ObtenerPorId(id);
            dbSet.Remove(categoria);
            _context.SaveChanges();
        }
        public IEnumerable<Categoria> ObtenerPorFiltro(CategoriaFilter filtro)
        {
            throw new NotImplementedException();
        }
    }
}

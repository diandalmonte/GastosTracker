using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Interfaces;
using Infraestructura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Persistencia.Repositorios
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly GastosTrackerContext _context;
        private readonly DbSet<T> dbSet;

        public Repository(GastosTrackerContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }
        public void Guardar(T entidad)
        {
            dbSet.Add(entidad);
            _context.SaveChanges();
        }

        public IEnumerable<T> Obtener()
        {
            return dbSet.ToList();
        }

        public T ObtenerPorId(Guid id)
        {
            return dbSet.Find(id);
        }

        public void Actualizar(T entidad)
        {
            dbSet.Entry(entidad).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Eliminar(Guid id)
        {
            var entidad = ObtenerPorId(id);
            dbSet.Remove(entidad);
            _context.SaveChanges();
        }
    }
}

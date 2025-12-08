using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Interfaces.Infraestructura;
using Infraestructura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Persistencia.Repositorios
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly GastosTrackerContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(GastosTrackerContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task Guardar(T entidad)
        {
            await _dbSet.AddAsync(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> Obtener()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T?> ObtenerPorId(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Actualizar(T entidad)
        {
            _dbSet.Update(entidad); 
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(Guid id)
        {
            var entidad = await ObtenerPorId(id);
            if (entidad != null)
            {
                _dbSet.Remove(entidad);
                await _context.SaveChangesAsync();
            }
        }
    }
}

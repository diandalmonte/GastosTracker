using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Interfaces.Infraestructura;
using Dominio.Modelos.Entidades;
using Infraestructura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Persistencia.Repositorios
{
    public class Repository<T> : IRepository<T> where T : class, IEntidadDeUsuario
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

        public async Task<IEnumerable<T>> Obtener(Guid idUsuario)
        {
            return await _dbSet.AsNoTracking().Where(t => t.UsuarioId == idUsuario).ToListAsync();
        }

        public async Task<T?> ObtenerPorId(Guid id, Guid idUsuario)
        {
            return await _dbSet.FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == idUsuario);
        }

        public async Task Actualizar(T entidad)
        {
            _dbSet.Update(entidad); 
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(Guid id, Guid idUsuario)
        {
            var entidad = await ObtenerPorId(id, idUsuario);
            if (entidad != null)
            {
                _dbSet.Remove(entidad);
                await _context.SaveChangesAsync();
            }
        }
    }
}

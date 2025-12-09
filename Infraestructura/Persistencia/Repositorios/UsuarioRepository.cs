using System;
using System.Collections.Generic;
using System.Linq;
using Dominio.Modelos.Entidades;
using Dominio.Exceptions;
using Infraestructura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using Aplicacion.Interfaces.Infraestructura;

namespace Infraestructura.Persistencia.Repositorios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly GastosTrackerContext _context;
        protected readonly DbSet<Usuario> _dbSet;

        public UsuarioRepository(GastosTrackerContext context)
        {
            _context = context;
            _dbSet = _context.Set<Usuario>();
        }

        public async Task<Usuario?> ObtenerPorId(Guid id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task Guardar(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(Guid id)
        {
            var usuario = await ObtenerPorId(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Usuario?> ObtenerPorEmail(string email)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public bool EmailExiste(string email)
        {
            return _context.Usuarios.Any(u => u.Email == email);
        }
    }
}

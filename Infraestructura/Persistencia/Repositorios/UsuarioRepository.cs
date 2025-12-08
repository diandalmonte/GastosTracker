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
    public class UsuarioRepository : IRepository<Usuario>, IUsuarioRepository
    {
        private readonly GastosTrackerContext _context;
        private readonly DbSet<Usuario> dbSet; 

        public UsuarioRepository(GastosTrackerContext context)
        {
            _context = context;
            dbSet = _context.Set<Usuario>();
        }

        public void Guardar(Usuario entidad)
        {
            dbSet.Add(entidad);
            _context.SaveChanges();
        }

        public IEnumerable<Usuario> Obtener()
        {
            return dbSet.ToList();
        }

        public Usuario? ObtenerPorId(Guid id)
        {
            return dbSet.Find(id);
        }

        public void Actualizar(Usuario entidad)
        {
            _context.Entry(entidad).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Eliminar(Guid id)
        {
            var entidad = ObtenerPorId(id);
            if (entidad == null) throw new IdNotFoundException("Id no fue encontrado para Usuario");

            dbSet.Remove(entidad);
            _context.SaveChanges();
        }

        public Usuario? ObtenerPorEmail(string email)
        {
            return dbSet.FirstOrDefault(u => u.Email == email);
        }
    }
}

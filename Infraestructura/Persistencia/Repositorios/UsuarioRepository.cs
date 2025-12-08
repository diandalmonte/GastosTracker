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
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(GastosTrackerContext context) : base(context) { }

        public async Task<Usuario?> ObtenerPorEmail(string email)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}

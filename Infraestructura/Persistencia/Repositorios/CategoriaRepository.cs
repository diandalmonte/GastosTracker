using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs;
using Aplicacion.Interfaces.Infraestructura;
using Dominio.Modelos.Entidades;
using Infraestructura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Persistencia.Repositorios
{
    public class CategoriaRepository : Repository<Categoria>, IFiltrableRepository<Categoria, string>
    {
        public CategoriaRepository(GastosTrackerContext context) : base(context) { }

        public async Task<IEnumerable<Categoria>> ObtenerPorFiltro(string filtro, Guid idUsuario)
        {
            var query = _dbSet.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                query = query.Where(c => c.UsuarioId == idUsuario && c.Nombre.Contains(filtro));
            }

            return await query.ToListAsync();
        }
    }
}

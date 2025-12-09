using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.GastoEntity;
using Aplicacion.Interfaces.Infraestructura;
using Dominio.Modelos.Entidades;
using Infraestructura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Persistencia.Repositorios
{
    public class GastoRepository : IRepository<Gasto>, IFiltrableRepository<Gasto, GastoFilter>
    {
        private readonly GastosTrackerContext _context;

        public GastoRepository(GastosTrackerContext context)
        {
            _context = context;
        }

        public async Task Guardar(Gasto gasto)
        {
            await _context.Gastos.AddAsync(gasto);
            await _context.SaveChangesAsync();
        }


        public async Task<Gasto?> ObtenerPorId(Guid id, Guid idUsuario)
        {
            return await _context.Gastos.Where(g => g.UsuarioId == idUsuario)
                .Include(g => g.Categoria) // Incluí categoria para facilitar proceso de Mapping a DTOs
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Gasto>> Obtener(Guid idUsuario)
        {
            return await _context.Gastos.Where(g => g.UsuarioId == idUsuario)
                .Include(g => g.Categoria) // Aqui tambien se incluye Categoria
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task Actualizar(Gasto gasto)
        {
            _context.Entry(gasto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }


        public async Task Eliminar(Guid id, Guid idUsuario)
        {
            // Aqui se busca directamente con FindAsync para evitar el .Include Categoria que realiza ObtenerPorId() (es mas rapido)
            var gasto = await _context.Gastos.FirstOrDefaultAsync(g => g.Id == id && g.UsuarioId == idUsuario);

            if (gasto != null)
            {
                _context.Gastos.Remove(gasto);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<Gasto>> ObtenerPorFiltro(GastoFilter filtro, Guid idUsuario)
        {
            var query = _context.Gastos.Where(g => g.UsuarioId == idUsuario)
                .Include(g => g.Categoria)
                .AsNoTracking()
                .AsQueryable(); //Uso de IQueryable para poder aplicar todas las consultas LINQ en forma de cascada

            if (!string.IsNullOrEmpty(filtro.ContieneString))
            {
                query = query.Where(g => g.Encabezado.Contains(filtro.ContieneString) ||
                                         (g.Descripcion != null && g.Descripcion.Contains(filtro.ContieneString)));
            }

            if (filtro.FechaInicio.HasValue)
            {
                query = query.Where(g => g.Fecha >= filtro.FechaInicio.Value);
            }

            if (filtro.FechaFin.HasValue)
            {
                query = query.Where(g => g.Fecha <= filtro.FechaFin.Value);
            }

            if (filtro.CategoriaId.HasValue)
            {
                query = query.Where(g => g.CategoriaId == filtro.CategoriaId);
            }

            if (filtro.MetodoDePagoId.HasValue)
            {
                query = query.Where(g => g.MetodoDePagoId == filtro.MetodoDePagoId);
            }

            return await query.ToListAsync();
        }
    }
}

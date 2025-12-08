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
    public class GastoRepository : IFiltrableRepository<Gasto, GastoFilter>
    {
        private readonly GastosTrackerContext _context;
        private readonly DbSet<Gasto> dbSet;

        public  GastoRepository(GastosTrackerContext context)
        {
            _context = context;
            dbSet = _context.Set<Gasto>();
        }

        public void Guardar(Gasto gasto)
        {
            dbSet.Add(gasto);
            _context.SaveChanges();
        }

        public Gasto ObtenerPorId(Guid id)
        {
            return dbSet.Find(id);
        }
        public IEnumerable<Gasto> Obtener()
        {
            return dbSet.ToList();
        }

        public void Actualizar(Gasto gasto)
        {
            dbSet.Entry(gasto).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Eliminar(Guid id)
        {
            Gasto gasto = ObtenerPorId(id);
            dbSet.Remove(gasto);
            _context.SaveChanges();
        }

        public IEnumerable<Gasto> ObtenerPorFiltro(GastoFilter filtro)
        {
            throw new NotImplementedException();
        }

    }
}

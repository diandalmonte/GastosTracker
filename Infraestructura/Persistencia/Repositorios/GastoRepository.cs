using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.Gasto;
using Aplicacion.Interfaces.Infraestructura;
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
            _context.Entry(gasto).State = EntityState.Modified;
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
            var query = _context.Gastos.AsQueryable(); //Uso de IQueryable para poder aplicar todas las consultas LINQ en forma de cascada

            if (!string.IsNullOrEmpty(filtro.ContieneString))
            {
                query = query.Where(g => g.Encabezado.Contains(filtro.ContieneString) ||
                                         g.Descripcion.Contains(filtro.ContieneString));
            }


            if (filtro.fechaInicio.HasValue)
            {
                query = query.Where(g => g.Fecha >= filtro.fechaInicio.Value);
            }

            if (filtro.fechaFin.HasValue)
            {
                query = query.Where(g => g.Fecha <= filtro.fechaFin.Value);
            }

            if (filtro.CategoriaId.HasValue)
            {
                query = query.Where(g => g.CategoriaId == filtro.CategoriaId);
            }

            if (filtro.MetodoDePagoId.HasValue)
            {
                query = query.Where(g => g.MetodoDePagoId == filtro.MetodoDePagoId);
            }

            return query.ToList();
        }


    }
  
}

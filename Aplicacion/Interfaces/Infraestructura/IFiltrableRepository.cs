using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.GastoEntity;
using Dominio.Modelos.Entidades;

namespace Aplicacion.Interfaces.Infraestructura
{
    public interface IFiltrableRepository<T, TFilter> : IRepository<T>
    {
        Task<IEnumerable<T>> ObtenerPorFiltro(TFilter filtro, Guid idUsuario);
    }
}

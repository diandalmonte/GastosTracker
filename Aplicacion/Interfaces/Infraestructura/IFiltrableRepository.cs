using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interfaces.Infraestructura
{
    public interface IFiltrableRepository<T, TFilter> : IRepository<T>
    {
        public IEnumerable<T> ObtenerPorFiltro(TFilter filtro);
    }
}

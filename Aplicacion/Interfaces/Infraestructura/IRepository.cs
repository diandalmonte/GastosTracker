using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interfaces.Infraestructura
{
    public interface IRepository<T>
    {
        public void Guardar(T entidad);
        public IEnumerable<T> Obtener();
        public T ObtenerPorId(Guid id);
        public void Actualizar(T entidad);
        public void Eliminar(Guid id);
    }
}

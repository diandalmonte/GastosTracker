using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interfaces
{
    public interface IRepository<T>
    {
        public void Guardar(T obj);
        public List<T> Obtener();
        public T ObtenerPorId(Guid Id);
        public void Actualizar(T obj);
        public void Eliminar(Guid Id);
    }
}

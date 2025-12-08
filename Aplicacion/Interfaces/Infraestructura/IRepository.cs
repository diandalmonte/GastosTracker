using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Modelos.Entidades;

namespace Aplicacion.Interfaces.Infraestructura
{
    public interface IRepository<T>
    {
        Task Guardar(T entidad);
        Task<IEnumerable<T>> Obtener();
        Task<T?> ObtenerPorId(Guid id);
        Task Actualizar(T entidad);
        Task Eliminar(Guid id);
    }
}

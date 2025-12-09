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
        Task<IEnumerable<T>> Obtener(Guid idUsuario);
        Task<T?> ObtenerPorId(Guid idEntidad, Guid idUsuario);
        Task Actualizar(T entidad);
        Task Eliminar(Guid idEntidad, Guid idUsuario);
    }
}

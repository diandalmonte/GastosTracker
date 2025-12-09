using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Modelos.Entidades;


namespace Aplicacion.Interfaces.Infraestructura
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObtenerPorId(Guid id);
        Task<List<Usuario>> Obtener(Guid id);
        Task Guardar(Usuario usuario);
        Task Actualizar(Usuario usuario);
        Task Eliminar(Guid id);
        bool EmailExiste(string email);
        Task<Usuario> ObtenerPorEmail(string email);
    }
}

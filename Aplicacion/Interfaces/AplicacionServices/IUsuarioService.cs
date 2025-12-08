using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.Usuario;

namespace Aplicacion.Interfaces.AplicacionServices
{
    public interface IUsuarioService
    {
        public void Guardar(UsuarioRequestDTO dto);
        public IEnumerable<UsuarioResponseDTO> ObtenerVistasPrevias();
        public UsuarioResponseDTO ObtenerPorId(Guid id);
        public void Actualizar(UsuarioRequestDTO dto);
        public void Eliminar(Guid id);
    }
}

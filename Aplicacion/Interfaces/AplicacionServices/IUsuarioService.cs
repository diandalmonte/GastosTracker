using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.UsuarioEntity;

namespace Aplicacion.Interfaces.AplicacionServices
{
    public interface IUsuarioService
    {
        public void Guardar(UsuarioRequestDTO dto);
        public UsuarioResponseDTO ObtenerPorId(Guid id);
        public void Actualizar(UsuarioRequestDTO dto);
        public void Eliminar(Guid id);
    }
}

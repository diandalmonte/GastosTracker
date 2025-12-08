using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs.UsuarioEntity
{
    public class UsuarioResponseDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
    }
}

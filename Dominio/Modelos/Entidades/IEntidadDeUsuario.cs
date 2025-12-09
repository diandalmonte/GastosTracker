using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Modelos.Entidades
{
    public interface IEntidadDeUsuario
    {
        Guid Id { get; set; }
        Guid UsuarioId { get; set; }
    }
}

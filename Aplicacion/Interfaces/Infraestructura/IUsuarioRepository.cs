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
        public Usuario? ObtenerPorEmail(string email);
    }
}

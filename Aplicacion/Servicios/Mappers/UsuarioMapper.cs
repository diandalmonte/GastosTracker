using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Modelos.Entidades;
using Aplicacion.DTOs.UsuarioEntity;
using Aplicacion.Interfaces.AplicacionServices;

namespace Aplicacion.Servicios.Mappers
{
    public class UsuarioMapper : IMapperService<Usuario, UsuarioRequestDTO, UsuarioResponseDTO>
    {
        public UsuarioResponseDTO MapDTO(Usuario ent)
        {
            return new UsuarioResponseDTO
            {
                Id = ent.Id,
                Nombre = ent.Nombre,
                Email = ent.Email
            };
        }

        public Usuario MapEntity(UsuarioRequestDTO dto)
        {
            return new Usuario
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = dto.Password
            };
        }
    }
}

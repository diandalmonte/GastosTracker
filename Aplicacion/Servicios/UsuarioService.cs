using System;
using System.Collections.Generic;
using System.Linq;
using Aplicacion.DTOs.UsuarioEntity;
using Aplicacion.Interfaces.AplicacionServices;
using Aplicacion.Interfaces.Infraestructura;
using Aplicacion.Exceptions;
using Dominio.Modelos.Entidades;

namespace Aplicacion.Servicios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;
        private readonly IMapperService<Usuario, UsuarioRequestDTO, UsuarioResponseDTO> _mapper;

        public UsuarioService(IUsuarioRepository repo, IMapperService<Usuario, UsuarioRequestDTO, UsuarioResponseDTO> mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public void Guardar(UsuarioRequestDTO dto)
        {
            if (!_repo.EmailExiste(dto.Email))
            {
                dto.Password = HashPassword(dto.Password);
                _repo.Guardar(_mapper.MapEntity(dto));
            }
            else
            {
                throw new EmailAlreadyInUseException("Email ya esta esta en uso");
            }
        }

        public UsuarioResponseDTO ObtenerPorId(Guid id)
        {
            var usuario = _repo.ObtenerPorId(id).Result;
            if (usuario == null)
            {
                throw new ItemNotFoundException($"Usuario con id {id} no encontrado.");
            }
            return _mapper.MapDTO(usuario);
        }

        public void Actualizar(UsuarioRequestDTO dto)
        {
            _repo.Actualizar(_mapper.MapEntity(dto));
        }

        public void Eliminar(Guid id)
        {
            var usuario = _repo.ObtenerPorId(id).Result;
            if (usuario == null) throw new ItemNotFoundException("Usuario no encontrado para eliminar.");

            _repo.Eliminar(id);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 2);
        }
    }
}

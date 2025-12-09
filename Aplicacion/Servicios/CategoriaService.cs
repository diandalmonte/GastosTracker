using System;
using System.Collections.Generic;
using System.Linq;
using Aplicacion.DTOs.CategoriaEntity;
using Aplicacion.Interfaces.AplicacionServices;
using Aplicacion.Interfaces.Infraestructura;
using Aplicacion.Exceptions; // Asumiendo que aquí están tus excepciones personalizadas
using Dominio.Modelos.Entidades;

namespace Aplicacion.Servicios
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IFiltrableRepository<Categoria, string> _repo;
        private readonly IMapperService<Categoria, CategoriaCreateDTO, CategoriaReadDTO> _mapper;

        public CategoriaService(IFiltrableRepository<Categoria, string> repo, IMapperService<Categoria, CategoriaCreateDTO, CategoriaReadDTO> mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public void Guardar(CategoriaCreateDTO dto)
        {
            // Validaciones de negocio podrían ir aquí antes de mapear
            var entidad = _mapper.MapEntity(dto);
            _repo.Guardar(entidad);
        }

        public async Task<IEnumerable<CategoriaReadDTO>> Obtener(Guid idUsuario)
        {
            IEnumerable<Categoria> categorias = await _repo.Obtener(idUsuario);
            return categorias.Select(c => _mapper.MapDTO(c));
        }

        public async Task<CategoriaReadDTO> ObtenerPorId(Guid id, Guid idUsuario)
        {
            var categoria = await _repo.ObtenerPorId(id, idUsuario);

            if (categoria == null)
            {
                throw new ItemNotFoundException($"No se encontró la categoría con id {id}");
            }

            return _mapper.MapDTO(categoria);
        }

        public async Task<IEnumerable<CategoriaReadDTO>> ObtenerPorFiltro(string filtro, Guid idUsuario)
        {
            var categorias =  await _repo.ObtenerPorFiltro(filtro, idUsuario);
            return categorias.Select(c => _mapper.MapDTO(c));
        }

        public void Actualizar(CategoriaCreateDTO dto)
        {
            var entidad = _mapper.MapEntity(dto);
            _repo.Actualizar(entidad);
        }

        public void Eliminar(Guid id, Guid idUsuario)
        {
            var categoria = _repo.ObtenerPorId(id, idUsuario).Result;
            if (categoria == null)
            {
                throw new ItemNotFoundException($"No se puede eliminar. La categoría {id} no existe.");
            }
            _repo.Eliminar(id, idUsuario);
        }
    }
}
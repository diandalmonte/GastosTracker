using System;
using System.Collections.Generic;
using System.Linq;
using Aplicacion.DTOs.MetodoDePagoEntity;
using Aplicacion.Interfaces.AplicacionServices;
using Aplicacion.Interfaces.Infraestructura;
using Aplicacion.Exceptions;
using Dominio.Modelos.Entidades;

namespace Aplicacion.Servicios
{
    public class MetodoDePagoService : IMetodoDePagoService
    {
        private readonly IRepository<MetodoDePago> _repo;
        private readonly IMapperService<MetodoDePago, MetodoDePagoCreateDTO, MetodoDePagoReadDTO> _mapper;

        public MetodoDePagoService(IRepository<MetodoDePago> repo, IMapperService<MetodoDePago, MetodoDePagoCreateDTO, MetodoDePagoReadDTO> mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public void Guardar(MetodoDePagoCreateDTO dto)
        {
            var entidad = _mapper.MapEntity(dto);
            _repo.Guardar(entidad);
        }

        public async Task<IEnumerable<MetodoDePagoReadDTO>> Obtener(Guid idUsuario)
        {
            var metodos = await _repo.Obtener(idUsuario);
            return metodos.Select(m => _mapper.MapDTO(m));
        }

        public async Task<MetodoDePagoReadDTO> ObtenerPorId(Guid id, Guid idUsuario)
        {
            var metodo = await _repo.ObtenerPorId(id, idUsuario);

            if (metodo == null)
            {
                throw new ItemNotFoundException($"Método de pago con id {id} no encontrado.");
            }

            return _mapper.MapDTO(metodo);
        }

        public void Actualizar(MetodoDePagoCreateDTO dto)
        {
            _repo.Actualizar(_mapper.MapEntity(dto));
        }

        public async void Eliminar(Guid id, Guid idUsuario)
        {
            var metodo = await _repo.ObtenerPorId(id, idUsuario);
            if (metodo == null) throw new ItemNotFoundException("El elemento a eliminar no existe.");

            await _repo.Eliminar(id, idUsuario);
        }
    }
}

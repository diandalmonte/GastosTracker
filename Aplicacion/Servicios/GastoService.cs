using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.GastoEntity;
using Aplicacion.Interfaces.AplicacionServices;
using Aplicacion.Exceptions;
using AutoMapper;
using Aplicacion.Interfaces.Infraestructura;
using Dominio.Modelos.Entidades;
using Aplicacion.Servicios.Mappers;

namespace Aplicacion.Servicios
{
    public class GastoService : IGastoService
    {
        private readonly IFiltrableRepository<Gasto, GastoFilter> _repo;
        private readonly IMapperService<Gasto, GastoCreateDTO, GastoReadDTO> _mapper;

        public GastoService(IFiltrableRepository<Gasto, GastoFilter> repo, IMapperService<Gasto, GastoCreateDTO, GastoReadDTO> mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public void Guardar(GastoCreateDTO cDto)
        {
            if (cDto.IsFechaActual)
            {
                cDto.Fecha = DateOnly.FromDateTime(DateTime.UtcNow);
            } 
            else if (!cDto.IsFechaActual && !cDto.Fecha.HasValue)
            {
                throw new ModelConstructionException("Error en la asignacion de fecha para Gasto");
            }

            _repo.Guardar(_mapper.MapEntity(cDto));

        }

        public async Task<IEnumerable<GastoVistaPrevia>> ObtenerPorFiltro(GastoFilter filtro, Guid idUsuario)
        {
            IEnumerable<Gasto> gastosFiltrados = await _repo.ObtenerPorFiltro(filtro, idUsuario);
            return [.. gastosFiltrados.Select(g => GastoMapper.MapVistaPrevia(g))];
        }

        public async Task<GastoReadDTO> ObtenerPorId(Guid id, Guid idUsuario)
        {
            Gasto? gasto = await _repo.ObtenerPorId(id, idUsuario);

            if (gasto == null)
            {
                throw new ItemNotFoundException("Id de gasto no fue encontrado");
            }

            return _mapper.MapDTO(gasto);
        }

        public async Task<PagedResult> ObtenerVistasPrevias(Guid idUsuario)
        {
            IEnumerable<Gasto> gastosFiltrados = await _repo.Obtener(idUsuario);
            /*PagedResult pagedResultGastos;
            return pagedResultGastos;*/
        }

        public void Actualizar(GastoCreateDTO cDto)
        {
            _repo.Actualizar(_mapper.MapEntity(cDto));
        }

        public void Eliminar(Guid id, Guid idUsuario)
        {
            _repo.Eliminar(id, idUsuario);
        }
    }
}

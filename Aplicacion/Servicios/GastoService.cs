using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.CategoriaEntity;
using Aplicacion.DTOs.GastoEntity;
using Aplicacion.Exceptions;
using Aplicacion.Interfaces.AplicacionServices;
using Aplicacion.Interfaces.Infraestructura;
using Aplicacion.Servicios.Mappers;
using AutoMapper;
using Dominio.Modelos.Entidades;

namespace Aplicacion.Servicios
{
    public class GastoService : IGastoService
    {
        private readonly IFiltrableRepository<Gasto, GastoFilter> _repo;
        private readonly IMapperService<Gasto, GastoCreateDTO, GastoReadDTO> _mapper;
        private readonly IPresupuestoService _presupuestoService;

        public GastoService(IFiltrableRepository<Gasto, GastoFilter> repo, IMapperService<Gasto, GastoCreateDTO, GastoReadDTO> mapper,
            IPresupuestoService presupuestoService)
        {
            _repo = repo;
            _mapper = mapper;
            _presupuestoService = presupuestoService;
        }
        public async Task<List<string>> Guardar(GastoCreateDTO cDto, bool isImported)
        {
            List<string> alertas;

            if (cDto.IsFechaActual)
            {
                cDto.Fecha = DateOnly.FromDateTime(DateTime.UtcNow);
            } 
            else if (!cDto.IsFechaActual && !cDto.Fecha.HasValue)
            {
                throw new ModelConstructionException("Error en la asignacion de fecha para Gasto");
            }

            if (!isImported)
            {
                alertas = await _presupuestoService.ProcesarGasto(cDto);
                await _repo.Guardar(_mapper.MapEntity(cDto));
                return alertas;
            }
            else
            {
                return [];
            }

            

        }
        public async Task<List<GastoVistaPrevia>> Obtener(Guid idUsuario)
        {
            var gastos = await _repo.Obtener(idUsuario);
            return gastos.Select(g => GastoMapper.MapVistaPrevia(g)).ToList();
            
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
            throw new NotImplementedException();
            /*IEnumerable<Gasto> gastosFiltrados = await _repo.Obtener(idUsuario);
            PagedResult pagedResultGastos;
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

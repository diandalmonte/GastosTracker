using System;
using System.Collections.Generic;
using System.Linq;
using Aplicacion.DTOs.CategoriaEntity;
using Aplicacion.Interfaces.AplicacionServices;
using Aplicacion.Interfaces.Infraestructura;
using Aplicacion.Exceptions; // Asumiendo que aquí están tus excepciones personalizadas
using Dominio.Modelos.Entidades;
using Aplicacion.DTOs.GastoEntity;
using Aplicacion.Utilidades;

namespace Aplicacion.Servicios
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IFiltrableRepository<Categoria, string> _repoCategoria;
        private readonly IMapperService<Categoria, CategoriaCreateDTO, CategoriaReadDTO> _mapper;
        private readonly IFiltrableRepository<Gasto, GastoFilter> _repoGastos;
        private readonly IPresupuestoService _presupuestoService;

        public CategoriaService(IFiltrableRepository<Categoria, string> repoCategorias,
            IMapperService<Categoria, CategoriaCreateDTO, CategoriaReadDTO> mapper,
            IFiltrableRepository<Gasto, GastoFilter> repoGastos,
            IPresupuestoService presupuestoService)
        {
            _repoCategoria = repoCategorias;
            _mapper = mapper;
            _repoGastos = repoGastos;
            _presupuestoService = presupuestoService;
        }

        public async Task Guardar(CategoriaCreateDTO dto)
        {
            if (await _presupuestoService.ProcesarCreacionCategoria(dto))
            {
                var entidad = _mapper.MapEntity(dto);
                await _repoCategoria.Guardar(entidad);
            }
            else
            {
                throw new OverBudgetException("El presupuesto para esta categoria se sale del Presupuesto Mensual");
            }
            
        }

        public async Task<IEnumerable<CategoriaReadDTO>> Obtener(Guid idUsuario)
        {
            return await _presupuestoService.ObtenerCategoriasProcesadas(idUsuario);
        }

        public async Task<CategoriaReadDTO> ObtenerPorId(Guid id, Guid idUsuario)
        {
            var categoria = await _repoCategoria.ObtenerPorId(id, idUsuario);

            if (categoria == null)
            {
                throw new ItemNotFoundException($"No se encontró la categoría con id {id}");
            }

            return _mapper.MapDTO(categoria);
        }

        public async Task<IEnumerable<CategoriaReadDTO>> ObtenerPorFiltro(string filtro, Guid idUsuario)
        {
            var categorias =  await _repoCategoria.ObtenerPorFiltro(filtro, idUsuario);
            return categorias.Select(c => _mapper.MapDTO(c));
        }

        public void Actualizar(CategoriaCreateDTO dto)
        {
            var entidad = _mapper.MapEntity(dto);
            _repoCategoria.Actualizar(entidad);
        }

        public void Eliminar(Guid id, Guid idUsuario)
        {
            var categoria = _repoCategoria.ObtenerPorId(id, idUsuario).Result;
            if (categoria == null)
            {
                throw new ItemNotFoundException($"No se puede eliminar. La categoría {id} no existe.");
            }
            _repoCategoria.Eliminar(id, idUsuario);
        }
    }
}
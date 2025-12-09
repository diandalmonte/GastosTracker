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

        public CategoriaService(IFiltrableRepository<Categoria, string> repoCategorias,
            IMapperService<Categoria, CategoriaCreateDTO, CategoriaReadDTO> mapper,
            IFiltrableRepository<Gasto, GastoFilter> repoGastos,
            IPresupuestoService presupuestoService)
        {
            _repoCategoria = repoCategorias;
            _mapper = mapper;
            _repoGastos = repoGastos;
        }

        public void Guardar(CategoriaCreateDTO dto)
        {
            // Validaciones de negocio podrían ir aquí antes de mapear
            var entidad = _mapper.MapEntity(dto);
            _repoCategoria.Guardar(entidad);
        }

        public async Task<IEnumerable<CategoriaReadDTO>> Obtener(Guid idUsuario)
        {

            var categorias = await _repoCategoria.Obtener(idUsuario);
            List<CategoriaReadDTO> listaDTOs = [];

            //Se calcula el rango del mes actual
            var (inicioMes, finMes) = DateExtensions.ObtenerRangoMesActual();

            //La logica siguiente es para poder brindar informacion extra a la UI
            foreach (var categoria in categorias)
            {
                var gastos = await _repoGastos.ObtenerPorFiltro(new GastoFilter 
                { 
                    FechaInicio = inicioMes, 
                    FechaFin = finMes, 
                    CategoriaId = categoria.Id 
                }, idUsuario);

                decimal totalGastado = gastos.Sum(g => g.Monto);
            
                var dto = _mapper.MapDTO(categoria);

                //Añadiendo esto al dto, ya que el Mapper no lo puede calcular/validar solo
                dto.IsExcedido = totalGastado > categoria.Presupuesto;
            
                if (categoria.Presupuesto > 0)
                {
                    dto.PorcentajePresupuesto = (int)((totalGastado / categoria.Presupuesto) * 100);
                }
                else 
                {
                    dto.PorcentajePresupuesto = 0;
                }

                listaDTOs.Add(dto);
            }

            return listaDTOs;
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
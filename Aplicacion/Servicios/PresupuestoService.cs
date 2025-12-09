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
using Aplicacion.Utilidades;
using Dominio.Modelos.Entidades;
using Dominio.Servicios;

namespace Aplicacion.Servicios
{
    public class PresupuestoService : IPresupuestoService
    {
        private readonly IFiltrableRepository<Categoria, string> _repoCategoria;
        private readonly IFiltrableRepository<Gasto, GastoFilter> _repoGastos;
        private readonly IUsuarioRepository _repoUsuarios;
        private readonly IMapperService<Categoria, CategoriaCreateDTO, CategoriaReadDTO> _mapper;
        private readonly IPresupuestoManager _presupuestoManager;
        private readonly ICategoriaService _categoriaService;


        public PresupuestoService(IFiltrableRepository<Categoria, string> repoCategoria,
            IFiltrableRepository<Gasto, GastoFilter> repoGastos, IUsuarioRepository repoUsuarios, 
            IMapperService<Categoria, CategoriaCreateDTO, CategoriaReadDTO> mapper,
            IPresupuestoManager presupuestoManager,
            ICategoriaService categoriaService)
        {
            _repoCategoria = repoCategoria;
            _repoGastos = repoGastos;
            _repoUsuarios = repoUsuarios;
            _mapper = mapper;
            _presupuestoManager = presupuestoManager;
            _categoriaService = categoriaService;
        }

        public async Task<List<CategoriaReadDTO>> ObtenerCategoriasProcesadas(Guid idUsuario)
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

                dto.PorcentajePresupuesto = (int) await ObtenerPorcentajeCategoria(categoria, idUsuario);

                listaDTOs.Add(dto);
            }

            return listaDTOs;
        }
        public async Task<List<CategoriaReadDTO>> ObtenerCategoriasExcedidas(Guid idUsuario)
        {
            IEnumerable<CategoriaReadDTO> todasLasCategorias = await ObtenerCategoriasProcesadas(idUsuario);

            //Filtramos las que esten excedidas
            return todasLasCategorias
                    .Where(c => c.IsExcedido)
                    .ToList();
        }

        public async Task<decimal> ObtenerDiferenciaGeneral(Guid idUsuario)
        {
            decimal presupuestoGeneral = await ObtenerPresupuestoGeneral(idUsuario);

            var (inicioMes, finMes) = DateExtensions.ObtenerRangoMesActual();

            IEnumerable<Gasto> gastosMes = await _repoGastos.ObtenerPorFiltro(new GastoFilter
            {
                FechaInicio = inicioMes,
                FechaFin = finMes,
            }, idUsuario);

            decimal totalGastado = gastosMes.Sum(g => g.Monto);

            return presupuestoGeneral - totalGastado;
        }
        private async Task<decimal> ObtenerPorcentajeCategoria(Categoria categoria, Guid idUsuario)
        {
            if (categoria.Presupuesto == 0) return 0;

            var (inicioMes, finMes) = DateExtensions.ObtenerRangoMesActual();

            IEnumerable<Gasto> gastosDeCategoria = await _repoGastos.ObtenerPorFiltro(new GastoFilter
            {
                FechaInicio = inicioMes,
                FechaFin = finMes,
                CategoriaId = categoria.Id
            }, idUsuario);

            decimal totalGastado = gastosDeCategoria.Sum(g => g.Monto);

            return (totalGastado / categoria.Presupuesto) * 100;
        }

        public async Task<List<string>> ProcesarGasto(GastoCreateDTO gasto)
        {
            var categoria = await _repoCategoria.ObtenerPorId(gasto.CategoriaId, gasto.UsuarioId);

            if (categoria == null)
                throw new ItemNotFoundException("La categoría del gasto no existe.");

            decimal presupuestoGeneral = await ObtenerPresupuestoGeneral(gasto.UsuarioId);

            DateOnly inicioMes, finMes;
            if (gasto.Fecha.HasValue)
            {
 
                var fechaReal = gasto.Fecha.Value;

                inicioMes = new DateOnly(fechaReal.Year, fechaReal.Month, 1);
                finMes = inicioMes.AddMonths(1).AddDays(-1);

            }
            else
            {
                throw new ModelConstructionException($"Gasto: {gasto.Encabezado} sin fecha");
            }

            var gastosCatList = await _repoGastos.ObtenerPorFiltro(new GastoFilter
            {
                FechaInicio = inicioMes,
                FechaFin = finMes,
                CategoriaId = categoria.Id
            }, gasto.UsuarioId);
 
            decimal acumuladoCategoria = gastosCatList.Sum(g => g.Monto);

            var gastosGenList = await _repoGastos.ObtenerPorFiltro(new GastoFilter
            {
                FechaInicio = inicioMes,
                FechaFin = finMes,
            }, gasto.UsuarioId);
            decimal acumuladoGeneral = gastosGenList.Sum(g => g.Monto);

            // Se llama al PresupuestoMaanager para generar las alertas
            return _presupuestoManager.ValidarPresupuesto(
                categoria,
                gasto.Monto,
                categoria.Presupuesto,
                acumuladoCategoria,
                presupuestoGeneral,
                acumuladoGeneral
            );
        }

        //helper methods para no tener que repetir codigo, solo el metodo.
        public async Task<decimal> ObtenerPresupuestoGeneral(Guid idUsuario) //hacer public?
        {
            Usuario? usuario = await _repoUsuarios.ObtenerPorId(idUsuario);
            if (usuario == null)
            {
                throw new ItemNotFoundException("Error id de Usuario no encontrado");
            }
            else
            {
                return usuario.Presupuesto;
            }
        }

        public async Task<int> ObtenerPorcentajePresupuestoGeneral(Guid idUsuario)
        {
            decimal presupuestoGeneral = await ObtenerPresupuestoGeneral(idUsuario);
            decimal totalgastos = presupuestoGeneral - await ObtenerDiferenciaGeneral(idUsuario);

            return (int) ((totalgastos / presupuestoGeneral) * 100);
        }

    }
}

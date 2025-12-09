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
using Dominio.Modelos.Entidades;

namespace Aplicacion.Servicios
{
    public class PresupuestoService : IPresupuestoService
    {
        private readonly IFiltrableRepository<Categoria, string> _repoCategoria;
        private readonly IFiltrableRepository<Gasto, GastoFilter> _repoGastos;
        private readonly IUsuarioRepository _repoUsuarios;


        public PresupuestoService(IFiltrableRepository<Categoria, string> repoCategoria,
            IFiltrableRepository<Gasto, GastoFilter> repoGastos)
        {
            _repoCategoria = repoCategoria;
            _repoGastos = repoGastos;
        }

        public async Task<List<CategoriaReadDTO>> ObtenerCategoriasExcedidas(Guid idUsuario)
        {
            decimal presupuestoGeneral = await ObtenerPresupuestoGeneral(idUsuario);
            IEnumerable<Categoria> categorias = await _repoCategoria.Obtener(idUsuario);

            foreach (Categoria categoria in categorias)
            {
                IEnumerable<T> _repoGastos.ObtenerPorFiltro(new GastoFilter { CategoriaId = categoria.Id})
            }
        }

        public decimal ObtenerDiferencia(Guid idUsuario)
        {
            throw new NotImplementedException();
        }

        public string ProcesarGasto(Gasto gasto)
        {
            throw new NotImplementedException();
        }

        //helper method para no tener que repetir codigo, solo el metodo.
        private async Task<decimal> ObtenerPresupuestoGeneral(Guid idUsuario)
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
    }
}

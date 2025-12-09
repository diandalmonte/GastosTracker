using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.CategoriaEntity;
using Dominio.Modelos.Entidades;

namespace Aplicacion.Interfaces.AplicacionServices
{
    public interface IPresupuestoService
    {
        Task<List<CategoriaReadDTO>> ObtenerCategoriasExcedidas(Guid idUsuario);
        Task<List<string>> ProcesarGasto(Gasto gasto);
        Task<decimal> ObtenerDiferenciaGeneral(Guid idUsuario);
        Task<decimal> ObtenerPorcentajeCategoria(Categoria categoria, Guid idUsuario);

    }
}

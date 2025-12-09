using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.CategoriaEntity;
using Aplicacion.DTOs.GastoEntity;
using Dominio.Modelos.Entidades;

namespace Aplicacion.Interfaces.AplicacionServices
{
    public interface IPresupuestoService
    {
        Task<List<CategoriaReadDTO>> ObtenerCategoriasProcesadas(Guid idUsuario);
        Task<List<CategoriaReadDTO>> ObtenerCategoriasExcedidas(Guid idUsuario);
        Task<List<string>> ProcesarGasto(GastoCreateDTO gasto);
        Task<decimal> ObtenerDiferenciaGeneral(Guid idUsuario);
        Task<bool> ProcesarCreacionCategoria(CategoriaCreateDTO gasto);

    }
}

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
        List<CategoriaReadDTO> ObtenerCategoriasExcedidas(Guid idUsuario);
        string ProcesarGasto(Gasto gasto);
        decimal ObtenerDiferencia(Guid idUsuario);

    }
}

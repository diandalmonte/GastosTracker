using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.GastoEntity;

namespace Aplicacion.Interfaces.Infraestructura
{
    public interface IGastoImport
    {
        Task<List<GastoCreateDTO>> ImportarExcel(Stream archivoStream, Guid usuarioId);
    }
}

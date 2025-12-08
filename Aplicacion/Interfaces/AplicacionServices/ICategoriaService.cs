using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.Categoria;

namespace Aplicacion.Interfaces.AplicacionServices
{
    public interface ICategoriaService
    {
        public void Guardar(CategoriaCreateDTO dto);
        public IEnumerable<CategoriaCreateDTO> Obtener();
        public CategoriaCreateDTO ObtenerPorId(Guid id);
        public void Actualizar(CategoriaCreateDTO dto);
        public void Eliminar(Guid id);
        public IEnumerable<CategoriaCreateDTO> ObtenerPorFiltro(string filtro);
    }
}

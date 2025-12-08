using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interfaces.AplicacionServices
{
    public interface IMapperService<TEntity, TCreateDto, TReadDto>
    {
        TReadDto MapDTO(TEntity ent);
        TEntity MapEntity(TCreateDto dto);
    }
}

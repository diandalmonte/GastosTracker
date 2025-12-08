using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.CategoriaEntity;
using Aplicacion.Interfaces.AplicacionServices;
using Dominio.Modelos.Entidades;

namespace Aplicacion.Servicios.Mappers
{
    public class CategoriaMapper : IMapperService<Categoria, CategoriaCreateDTO, CategoriaReadDTO>
    {
        public CategoriaReadDTO MapDTO(Categoria ent)
        {
            return new CategoriaReadDTO
            {
                Id = ent.Id,
                Nombre = ent.Nombre,
                MontoPresupuesto = ent.Presupuesto?.MontoLimite,
                PorcentajePresupuesto = null,
                IsExcedido = false
            };
        }

        public Categoria MapEntity(CategoriaCreateDTO dto)
        {
            //El presupuestoId se agrega en CategoriaService, por lo que aqui solo se creara con ese campo vacio.
            return new Categoria(dto.Nombre, Guid.Empty, dto.UsuarioId);
        }
    }
}

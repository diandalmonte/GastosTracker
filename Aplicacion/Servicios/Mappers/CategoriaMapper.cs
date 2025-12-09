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
                MontoPresupuesto = ent.Presupuesto,
                PorcentajePresupuesto = null,
                IsExcedido = false
            };
        }

        public Categoria MapEntity(CategoriaCreateDTO dto)
        { 
            return new Categoria(dto.Nombre, dto.MontoPresupuesto, dto.UsuarioId);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.GastoEntity;
using Aplicacion.Interfaces.AplicacionServices;
using Dominio.Modelos.Entidades;

namespace Aplicacion.Servicios.Mappers
{
    public class GastoMapper : IMapperService<Gasto, GastoCreateDTO, GastoReadDTO>
    {
        public GastoReadDTO MapDTO(Gasto ent)
        {
            return new GastoReadDTO
            {
                Id = ent.Id,
                Encabezado = ent.Encabezado,
                Monto = ent.Monto,
                Fecha = ent.Fecha,
                Descripcion = ent.Descripcion,
                NombreCategoria = ent.Categoria?.Nombre ?? "Sin Categoría",
                MetodoDePago = ent.MetodoDePago?.Nombre ?? "Desconocido"
            };
        }

        // Metodo extra para la vista previa
        public static GastoVistaPrevia MapVistaPrevia(Gasto ent)
        {
            return new GastoVistaPrevia
            {
                Id = ent.Id,
                Encabezado = ent.Encabezado,
                Monto = ent.Monto,
                NombreCategoria = ent.Categoria?.Nombre ?? "Sin Categoría"
            };
        }

        public Gasto MapEntity(GastoCreateDTO dto)
        {
            if (dto.Fecha == null)
            {
                DateOnly fechaGasto = DateOnly.FromDateTime(DateTime.Now);
                return new Gasto(dto.Encabezado, dto.Monto, dto.CategoriaId, dto.MetodoDePagoId, dto.UsuarioId, dto.Descripcion)
                {
                    Fecha = fechaGasto
                };
            }
            else
            {
                DateOnly fecha = (DateOnly)dto.Fecha;
                return new Gasto(dto.Encabezado, dto.Monto, dto.CategoriaId, dto.MetodoDePagoId, dto.UsuarioId, fecha, dto.Descripcion);
            }



        }
    }
}

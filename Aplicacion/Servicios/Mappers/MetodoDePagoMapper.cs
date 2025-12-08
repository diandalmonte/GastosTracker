using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.MetodoDePagoEntity;
using Aplicacion.Interfaces.AplicacionServices;
using Dominio.Modelos.Entidades;
using Dominio.Modelos.Enums;

namespace Aplicacion.Servicios.Mappers
{
    public class MetodoDePagoMapper : IMapperService<MetodoDePago, MetodoDePagoCreateDTO, MetodoDePagoReadDTO>
    {
        public MetodoDePagoReadDTO MapDTO(MetodoDePago ent)
        {
            return new MetodoDePagoReadDTO
            {
                Id = ent.Id,
                Nombre = ent.Nombre,
                TipoPago = ent.TipoDePago.ToString()
            };
        }

        public MetodoDePago MapEntity(MetodoDePagoCreateDTO dto)
        {
            TipoPago tipoPagoEnum;
            //Validacion extra, solo por si acaso, ya que TipoPago no sera modificable por el usuario
            if (!Enum.TryParse(dto.TipoPago, true, out tipoPagoEnum))
            {
                throw new ArgumentException($"El tipo de pago '{dto.TipoPago}' no es válido.");
            }

            return new MetodoDePago
            {
                Nombre = dto.Nombre,
                TipoDePago = tipoPagoEnum,
                UsuarioId = dto.UsuarioId
            };
        }
    }
}

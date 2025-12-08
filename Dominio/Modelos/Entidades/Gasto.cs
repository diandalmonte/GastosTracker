using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dominio.Exceptions;

namespace Dominio.Modelos.Entidades
{
    public class Gasto : EntidadBase //Does use of required make sense here?
    {
        public string Encabezado { get; set; }
        public decimal Monto { get; private set; } //CAMBIAR: Validar que esto sea solo positivo
        public Guid CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }//PORQUE CATEGORIA, METODO, ETC SON ? SI SON OBLIGATORIAS

        public Guid MetodoDePagoId { get; set; }
        [ForeignKey("MetodoDePagoId")]
        public MetodoDePago? MetodoDePago { get; set; }

        public Guid UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }
        public string? Descripcion { get; set; }
        public DateOnly Fecha { get; set; }


        public Gasto(string encabezado, decimal monto, Guid categoriaId, Guid metodoDePagoId, Guid usuarioId, DateOnly fecha, string? descripcion)
        {
            SetMonto(monto);

            Encabezado = encabezado;
            Monto = monto;
            CategoriaId = categoriaId;
            MetodoDePagoId = metodoDePagoId;
            UsuarioId = usuarioId;
            Fecha = fecha;
            Descripcion = descripcion;
        }


        private void SetMonto(decimal monto)
        {
            if (monto <= 0)
            {

                throw new NegativeValueException("El monto de un gasto debe ser positivo.");
            }
            else
            {
                this.Monto = monto;
            }
        }
    }
}

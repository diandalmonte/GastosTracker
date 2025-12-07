using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dominio.Exceptions;

namespace Dominio.Modelos.Entidades
{
    public class Gasto : EntidadBase //Does use of required make sense here?
    {
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
        public DateTime Fecha { get; set; }


        public Gasto(decimal monto, Guid categoriaId, Guid metodoDePagoId, Guid usuarioId, DateTime fecha, string? descripcion)
        {
            SetMonto(monto);

            this.Monto = monto;
            this.CategoriaId = categoriaId;
            this.MetodoDePagoId = metodoDePagoId;
            this.UsuarioId = usuarioId;
            this.Fecha = fecha;
            this.Descripcion = descripcion;
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

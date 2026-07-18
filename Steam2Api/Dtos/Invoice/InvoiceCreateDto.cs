using System.ComponentModel.DataAnnotations;

namespace Steam2Api.Dtos.Invoice
{
    public class InvoiceCreateDto
    {
        [Required(ErrorMessage = "El ID del usuario es obligatorio")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "La fecha de la factura es obligatoria")]
        public DateTime InvoiceDate { get; set; }

        [Required(ErrorMessage = "El total es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El total debe ser mayor o igual a 0")]
        public decimal Total { get; set; }
    }
}